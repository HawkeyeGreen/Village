using Microsoft.Xna.Framework;

namespace Village.VillageGame.CombatSystem
{
    struct AttackDirection
    {
        private Vector3 relativAttackDirection;
        public Vector3 RelativAttackVektor => relativAttackDirection;

        public AttackDirection(Vector3 aimVektor)
        {
            relativAttackDirection = aimVektor;
        }
    }

    static class AttackDirections
    {
        #region Konstanten - Vektoren
        public static Vector3 _ORIGIN = new Vector3(0, 0, 0);

        #region Frontal Attacks: Face-to-face
        public static Vector3 _FRONT_FRONTAL = new Vector3(1, 0, 0);
        public static Vector3 _FRONT_FRONTAL_UPPER = new Vector3(1, 0, 0.5f);
        public static Vector3 _FRONT_FRONTAL_LOWER = new Vector3(1, 0, -0.5f);

        public static Vector3 _FRONT_LEFTSWING = new Vector3(1, 0.5f, 0);
        public static Vector3 _FRONT_LEFT_UPPER_SWING = new Vector3(1, 0.5f, 0.5f);
        public static Vector3 _FRONT_LEFT_LOWER_SWING = new Vector3(1, 0.5f, -0.5f);

        public static Vector3 _FRONT_RIGHTSWING = new Vector3(1, -0.5f, 0);
        public static Vector3 _FRONT_RIGHT_UPPER_SWING = new Vector3(1, -0.5f, 0.5f);
        public static Vector3 _FRONT_RIGHT_LOWER_SWING = new Vector3(1, -0.5f, -0.5f);
        #endregion
        
        #region Side Attacks: Face-to-Side (Lefthand from Defender)
        public static Vector3 _LEFTHAND_FRONTAL = new Vector3(0, -1, 0);
        public static Vector3 _LEFTHAND_FRONTAL_UPPER = new Vector3(0, -1, 0.5f);
        public static Vector3 _LEFTHAND_FRONTAL_LOWER = new Vector3(0, -1, -0.5f);

        public static Vector3 _LEFTHAND_LEFTSWING = new Vector3(0.5f, -1, 0);
        public static Vector3 _LEFTHAND_LEFT_UPPER_SWING = new Vector3(0.5f, -1, 0.5f);
        public static Vector3 _LEFTHAND_LEFT_LOWER_SWING = new Vector3(0.5f, -1, -0.5f);

        public static Vector3 _LEFTHAND_RIGHTSWING = new Vector3(-0.5f, -1, 0);
        public static Vector3 _LEFTHAND_RIGHT_UPPER_SWING = new Vector3(-0.5f, -1, 0.5f);
        public static Vector3 _LEFTHAND_RIGHT_LOWER_SWING = new Vector3(-0.5f, -1, -0.5f);
        #endregion

        #region Side Attacks: Face-to-Side (Righthand from Defender)
        public static Vector3 _RIGHTHAND_FRONTAL = new Vector3(0, 1, 0);
        public static Vector3 _RIGHTHAND_FRONTAL_UPPER = new Vector3(0, 1, 0.5f);
        public static Vector3 _RIGHTHAND_FRONTAL_LOWER = new Vector3(0, 1, -0.5f);

        public static Vector3 _RIGHTHAND_LEFTSWING = new Vector3(0.5f, 1, 0);
        public static Vector3 _RIGHTHAND_LEFT_UPPER_SWING = new Vector3(0.5f, 1, 0.5f);
        public static Vector3 _RIGHTHAND_LEFT_LOWER_SWING = new Vector3(0.5f, 1, -0.5f);

        public static Vector3 _RIGHTHAND_RIGHTSWING = new Vector3(-0.5f, 1, 0);
        public static Vector3 _RIGHTHAND_RIGHT_UPPER_SWING = new Vector3(-0.5f, 1, 0.5f);
        public static Vector3 _RIGHTHAND_RIGHT_LOWER_SWING = new Vector3(-0.5f, 1, -0.5f);
        #endregion

        #region Back Attacks: Face-to-Back (Backstabbin' little fuck'r)
        public static Vector3 _BACK_FRONTAL = new Vector3(-1, 0, 0);
        public static Vector3 _BACK_FRONTAL_UPPER = new Vector3(-1, 0, 0.5f);
        public static Vector3 _BACK_FRONTAL_LOWER = new Vector3(-1, 0, -0.5f);

        public static Vector3 _BACK_LEFTSWING = new Vector3(-1, 0.5f, 0);
        public static Vector3 _BACK_LEFT_UPPER_SWING = new Vector3(-1, 0.5f, 0.5f);
        public static Vector3 _BACK_LEFT_LOWER_SWING = new Vector3(-1, 0.5f, -0.5f);

        public static Vector3 _BACK_RIGHTSWING = new Vector3(-1, -0.5f, 0);
        public static Vector3 _BACK_RIGHT_UPPER_SWING = new Vector3(-1, -0.5f, 0.5f);
        public static Vector3 _BACK_RIGHT_LOWER_SWING = new Vector3(-1, -0.5f, -0.5f);
        #endregion

        #region OnTop Attacks: Face-to-Head
        public static Vector3 _TOP_FRONTAL = new Vector3(0, 0, 1);
        public static Vector3 _TOP_FRONTAL_UPPER = new Vector3(0, 0.5f, 1);
        public static Vector3 _TOP_FRONTAL_LOWER = new Vector3(0, -0.5f, 1);

        public static Vector3 _TOP_LEFTSWING = new Vector3(-0.5f, 0, 1);
        public static Vector3 _TOP_LEFT_UPPER_SWING = new Vector3(-0.5f, 0.5f, 1);
        public static Vector3 _TOP_LEFT_LOWER_SWING = new Vector3(-0.5f, -0.5f, 1);

        public static Vector3 _TOP_RIGHTSWING = new Vector3(0.5f, 0, 1);
        public static Vector3 _TOP_RIGHT_UPPER_SWING = new Vector3(0.5f, 0.5f, 1);
        public static Vector3 _TOP_RIGHT_LOWER_SWING = new Vector3(0.5f, -0.5f, 1);
        #endregion

        #region FromDownunder Attacks: Face-to-Feet
        public static Vector3 _BOTTOM_FRONTAL = new Vector3(0, 0, -1);
        public static Vector3 _BOTTOM_FRONTAL_UPPER = new Vector3(0, 0.5f, -1);
        public static Vector3 _BOTTOM_FRONTAL_LOWER = new Vector3(0, -0.5f, -1);

        public static Vector3 _BOTTOM_LEFTSWING = new Vector3(-0.5f, 0, -1);
        public static Vector3 _BOTTOM_LEFT_UPPER_SWING = new Vector3(-0.5f, 0.5f, -1);
        public static Vector3 _BOTTOM_LEFT_LOWER_SWING = new Vector3(-0.5f, -0.5f, -1);

        public static Vector3 _BOTTOM_RIGHTSWING = new Vector3(0.5f, 0, -1);
        public static Vector3 _BOTTOM_RIGHT_UPPER_SWING = new Vector3(0.5f, 0.5f, -1);
        public static Vector3 _BOTTOM_RIGHT_LOWER_SWING = new Vector3(0.5f, -0.5f, -1);
        #endregion

        #endregion

        #region Konstanten - strings
        public static string ORIGIN = "Origin";

        #region Frontal Attacks: Face-to-face
        public static string FRONT_FRONTAL = "Frontal Center Attack";
        public static string FRONT_FRONTAL_UPPER = "Frontal Center Attack From Top-Down";
        public static string FRONT_FRONTAL_LOWER = "Frontal Center Attack From Down-Top";

        public static string FRONT_LEFTSWING = "Frontal Attack Leftswung";
        public static string FRONT_LEFT_UPPER_SWING = "Frontal Leftside Up-Attack";
        public static string FRONT_LEFT_LOWER_SWING = "Frontal Leftside Down-Attack";

        public static string FRONT_RIGHTSWING = "Frontal Attack Rightswung";
        public static string FRONT_RIGHT_UPPER_SWING = "Frontal Rightside Up-Attack";
        public static string FRONT_RIGHT_LOWER_SWING = "Frontal Rightside Down-Attack";
        #endregion

        #region Lefthand Attacks: Face-to-Side
        public static string LEFTHAND_FRONTAL = "Frontal Center Attack";
        public static string LEFTHAND_FRONTAL_UPPER = "Frontal Center Attack From Top-Down";
        public static string LEFTHAND_FRONTAL_LOWER = "Frontal Center Attack From Down-Top";

        public static string LEFTHAND_LEFTSWING = "Frontal Attack Leftswung";
        public static string LEFTHAND_LEFT_UPPER_SWING = "Frontal Leftside Up-Attack";
        public static string LEFTHAND_LEFT_LOWER_SWING = "Frontal Leftside Down-Attack";

        public static string LEFTHAND_RIGHTSWING = "Frontal Attack Rightswung";
        public static string LEFTHAND_RIGHT_UPPER_SWING = "Frontal Rightside Up-Attack";
        public static string LEFTHAND_RIGHT_LOWER_SWING = "Frontal Rightside Down-Attack";
        #endregion

        #region Righthand Attacks: Face-to-Side
        public static string RIGHTHAND_FRONTAL = "Frontal Center Attack";
        public static string RIGHTHAND_FRONTAL_UPPER = "Frontal Center Attack From Top-Down";
        public static string RIGHTHAND_FRONTAL_LOWER = "Frontal Center Attack From Down-Top";

        public static string RIGHTHAND_LEFTSWING = "Frontal Attack Leftswung";
        public static string RIGHTHAND_LEFT_UPPER_SWING = "Frontal Leftside Up-Attack";
        public static string RIGHTHAND_LEFT_LOWER_SWING = "Frontal Leftside Down-Attack";

        public static string RIGHTHAND_RIGHTSWING = "Frontal Attack Rightswung";
        public static string RIGHTHAND_RIGHT_UPPER_SWING = "Frontal Rightside Up-Attack";
        public static string RIGHTHAND_RIGHT_LOWER_SWING = "Frontal Rightside Down-Attack";
        #endregion



        #endregion

        public static string ConvertAttackDirection(AttackDirection attackDirection)
        {
            if (attackDirection.RelativAttackVektor == _ORIGIN)
            {
                return ORIGIN;
            }

            #region Frontalattacks
            if (attackDirection.RelativAttackVektor == _FRONT_FRONTAL) { return FRONT_FRONTAL; }
            if (attackDirection.RelativAttackVektor == _FRONT_FRONTAL_UPPER) { return FRONT_FRONTAL_UPPER; }
            if (attackDirection.RelativAttackVektor == _FRONT_FRONTAL_LOWER) { return FRONT_FRONTAL_LOWER; }

            if (attackDirection.RelativAttackVektor == _FRONT_LEFTSWING) { return FRONT_LEFTSWING; }
            if (attackDirection.RelativAttackVektor == _FRONT_LEFT_UPPER_SWING) { return FRONT_LEFT_UPPER_SWING; }
            if (attackDirection.RelativAttackVektor == _FRONT_LEFT_LOWER_SWING) { return FRONT_LEFT_LOWER_SWING; }

            if (attackDirection.RelativAttackVektor == _FRONT_RIGHTSWING) { return FRONT_RIGHTSWING; }
            if (attackDirection.RelativAttackVektor == _FRONT_RIGHT_UPPER_SWING) { return FRONT_RIGHT_UPPER_SWING; }
            if (attackDirection.RelativAttackVektor == _FRONT_RIGHT_LOWER_SWING) { return FRONT_RIGHT_LOWER_SWING; }
            #endregion

            #region Lefthandattacks
            if (attackDirection.RelativAttackVektor == _LEFTHAND_FRONTAL) { return LEFTHAND_FRONTAL; }
            if (attackDirection.RelativAttackVektor == _LEFTHAND_FRONTAL_UPPER) { return LEFTHAND_FRONTAL_UPPER; }
            if (attackDirection.RelativAttackVektor == _LEFTHAND_FRONTAL_LOWER) { return LEFTHAND_FRONTAL_LOWER; }

            if (attackDirection.RelativAttackVektor == _LEFTHAND_LEFTSWING) { return LEFTHAND_LEFTSWING; }
            if (attackDirection.RelativAttackVektor == _LEFTHAND_LEFT_UPPER_SWING) { return LEFTHAND_LEFT_UPPER_SWING; }
            if (attackDirection.RelativAttackVektor == _LEFTHAND_LEFT_LOWER_SWING) { return LEFTHAND_LEFT_LOWER_SWING; }

            if (attackDirection.RelativAttackVektor == _LEFTHAND_RIGHTSWING) { return LEFTHAND_RIGHTSWING; }
            if (attackDirection.RelativAttackVektor == _LEFTHAND_RIGHT_UPPER_SWING) { return LEFTHAND_RIGHT_UPPER_SWING; }
            if (attackDirection.RelativAttackVektor == _LEFTHAND_RIGHT_LOWER_SWING) { return LEFTHAND_RIGHT_LOWER_SWING; }
            #endregion

            #region Righthandattacks
            if (attackDirection.RelativAttackVektor == _RIGHTHAND_FRONTAL) { return RIGHTHAND_FRONTAL; }
            if (attackDirection.RelativAttackVektor == _RIGHTHAND_FRONTAL_UPPER) { return RIGHTHAND_FRONTAL_UPPER; }
            if (attackDirection.RelativAttackVektor == _RIGHTHAND_FRONTAL_LOWER) { return RIGHTHAND_FRONTAL_LOWER; }

            if (attackDirection.RelativAttackVektor == _RIGHTHAND_LEFTSWING) { return RIGHTHAND_LEFTSWING; }
            if (attackDirection.RelativAttackVektor == _RIGHTHAND_LEFT_UPPER_SWING) { return RIGHTHAND_LEFT_UPPER_SWING; }
            if (attackDirection.RelativAttackVektor == _RIGHTHAND_LEFT_LOWER_SWING) { return RIGHTHAND_LEFT_LOWER_SWING; }

            if (attackDirection.RelativAttackVektor == _RIGHTHAND_RIGHTSWING) { return RIGHTHAND_RIGHTSWING; }
            if (attackDirection.RelativAttackVektor == _RIGHTHAND_RIGHT_UPPER_SWING) { return RIGHTHAND_RIGHT_UPPER_SWING; }
            if (attackDirection.RelativAttackVektor == _RIGHTHAND_RIGHT_LOWER_SWING) { return RIGHTHAND_RIGHT_LOWER_SWING; }
            #endregion



            return "UNBEKANNTE RICHTUNG!";
        }
    }
}