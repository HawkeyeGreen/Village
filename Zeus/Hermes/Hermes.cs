using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace Zeus.Hermes
{
    /* Hermes - General Purpose Messaging System
     * Hermes sammelt Zustandsmeldung etc. von beliebigen GameObjects und sammelt
     * diese sowohl in einem geschriebenen Log als auch in einer zur Laufzeit verfügbaren Sammlung.
     * 
     * Darüber hinaus stehen das HermesBlackboard für die Kommunikation zwischen verschiedenen Programmteilen
     * und das PlayerMessage-Log zu Verfügung. Letzteres dient dafür Meldungen aufzunehmen, die für den SC relevant sind.
     * 
     */
    public class Hermes
    {
        private static int debugLevel = 4;
        private static Hermes instance = null;

        private static Object instanceLock = new Object();
        private Object blackboardLock;
        private Object mainLogLock;
        private Object queueMainLogLock;


        private Dictionary<HermesLoggable, List<Tuple<string, HermesLoggable>>> blackboard;

        private Dictionary<HermesLoggable, string> mainLog;
        private HermesMainLogger mainLogger;
        private Queue<Tuple<HermesLoggable, string, int>> queueMainLog;
        private Thread mainLogThread;

        private Hermes()
        {
            blackboard = new Dictionary<HermesLoggable, List<Tuple<string, HermesLoggable>>>();
            blackboardLock = new Object();

            mainLog = new Dictionary<HermesLoggable, string>();
            mainLogLock = new Object();

            queueMainLog = new Queue<Tuple<HermesLoggable, string, int>>();
            queueMainLogLock = new Object();

            mainLogger = new HermesMainLogger(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\MainLog.txt", queueMainLog, queueMainLogLock);
            mainLogThread = new Thread(new ThreadStart(mainLogger.log));
            mainLogThread.Start();

            log(new SystemDummy("Hermes"), "Hermes system was started.", 0);
        }

        /// <summary> 
        /// Gets the one and only instance of the Hermes-Logger.
        /// </summary> 
        public static Hermes GetInstance()
        {
            if(instance == null)
            {
                lock(instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new Hermes();
                    }
                }
            }
            return instance;
        }


        #region BlackBoard
        public void lodgeBlackboardMessage(HermesLoggable sender, HermesLoggable receiver, string message)
        {
            lock(blackboardLock)
            {
                if (!blackboard.ContainsKey(receiver))
                {
                    blackboard.Add(receiver,new List<Tuple<string, HermesLoggable>>());
                }
                blackboard[receiver].Add(new Tuple<string, HermesLoggable>(message, sender));
            }

            Console.WriteLine("BlackboardMessage lodged from " + sender.ID.ToString() + " of type " + sender.Type + " for " + receiver.ID.ToString() + "! Message" + message);
        }

        public List<Tuple<string, HermesLoggable>> getBlackBoardMessages(HermesLoggable receiver)
        {
            lock (blackboardLock)
            {
                List<Tuple<string, HermesLoggable>> messages = blackboard[receiver];
                blackboard.Remove(receiver);
                return messages;
            }
        }

        public bool lookForBlackBoardMessages(HermesLoggable receiver)
        {
            lock (blackboardLock)
            {
                return blackboard.ContainsKey(receiver);
            }
        }
        #endregion

        #region MainLog
        /// <summary> 
        /// Logs a message to the main.log. This overload needs a HermesLoggable as sender.
        /// </summary> 
        public void log(HermesLoggable sender, string entry, int level = 5)
        {
            if(level < debugLevel)
            {
                lock (queueMainLogLock)
                {
                    queueMainLog.Enqueue(new Tuple<HermesLoggable, string, int>(sender, entry, level));
                }
            }

        }

        /// <summary> 
        /// For this overload of log a SystemDummy will be created with the ID 1337 and the optional name of <paramref name="sender"/>
        /// </summary> 
        public void log(string entry, string sender = "System", int level = 5)
        {
            if(level < debugLevel)
            {
                lock (queueMainLogLock)
                {
                    queueMainLog.Enqueue(new Tuple<HermesLoggable, string, int>(new SystemDummy(sender), entry, level));
                }
            }
        }
        #endregion

        /// <summary>
        /// Shutsdown the Hermes-System. You need to call this method so the System doesn't block the end of your programm.
        /// </summary>
        public void shutdownHermes()
        {
            log(new SystemDummy("Hermes"), "Shutting down... Bye bye.", 0);
            log(new SystemDummy("Hermes"), "ShutdownSignal", 0);
            while (!mainLogger.ended)
            {

            }
            mainLogThread.Abort();
        }
    }

    class HermesMainLogger : HermesLoggable
    {
        private int fileCounter = 0;
        private int lineCounter = 0;
        private int currentDebugLevel = 5;

        private string path;

        private StreamWriter writer;
        private Object queueLock;
        private Queue<Tuple<HermesLoggable, string, int>> queue;

        private bool run = true;
        public bool Running { get => run; set => run = value; }

        public long ID => 1337;
        public string Type => "Main Logger-Service";

        public bool ended = false;

        public HermesMainLogger(string filePath, Queue<Tuple<HermesLoggable, string, int>> _Queue, Object Lock)
        {
            if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\");
            }

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Archiv\\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Archiv\\");
            }

            if (File.Exists(filePath))
            {
                string newPath = filePath.Replace("MainLog", "\\Archiv\\MainLog_Archived " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                File.Move(filePath, newPath);
            }

            path = filePath;
            string tmpPath = filePath.Replace("MainLog","MainLog 1");
            int counter = 1;

            while(File.Exists(tmpPath))
            {
                string newPath = tmpPath.Replace("MainLog " + counter, "\\Archiv\\MainLog " + counter + "_Archived " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                File.Move(tmpPath, newPath);
                counter++;
                tmpPath = tmpPath.Replace("MainLog " + (counter - 1), "MainLog " + counter);
            }

            writer = new StreamWriter(File.OpenWrite(filePath));

            queue = _Queue;
            queueLock = Lock;
        }

        public void log()
        {
            #region do Logging
            while(run)
            {
                while (queue.Count == 0)
                {
                    System.Threading.Thread.Sleep(100);
                }

                lock(queueLock)
                {
                    Tuple<HermesLoggable, string, int> entry = queue.Dequeue();
                    if(entry.Item2 == "ShutdownSignal")
                    {
                        Running = false;
                    }
                    string logEntry = "|DEBUG_LEVEL:" + entry.Item3 + "| " + entry.Item1.ID.ToString() + " | " + entry.Item1.Type + ": " + entry.Item2;
                    int debugL = entry.Item3;
                    Console.WriteLine(logEntry);
                    if(debugL < currentDebugLevel)
                    {
                        lineCounter++;
                        if (lineCounter < 25000)
                        {
                            writer.WriteLine(logEntry);
                        }
                        else
                        {
                            writer.Close();
                            fileCounter++;
                            lineCounter = 0;
                            writer = new StreamWriter(File.OpenWrite(path.Replace("MainLog", "MainLog " + fileCounter)));
                            writer.WriteLine(logEntry);
                        }
                    }
                }
            }
            #endregion

            #region stopLogger
            writer.Close();

            ended = true;
            #endregion
        }
    }

    
}
