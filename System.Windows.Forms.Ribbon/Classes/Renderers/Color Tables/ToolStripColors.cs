//using Microsoft.VisualBasic;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;

//namespace System.Windows.Forms
//{
//    public class ToolStripColors
//    {
//        public static System.Drawing.Color clrVerBG_Shadow = System.Drawing.Color.FromArgb(255, 181, 190, 206);

//        public static System.Drawing.Color highlightColor = System.Drawing.Color.FromArgb(125, 255, 255, 0);
//        //This value should be between 0 and 255

//        public static int _Opacity = 255;
//        #region "Fore Colors"
//        public static System.Drawing.Color clrBtn_FColorPressed_Selected = System.Drawing.Color.FromArgb(0, 25, 56);
//        public static System.Drawing.Color clrBtn_FColor = System.Drawing.Color.FromArgb(255, 255, 255);
//        #endregion
//        public static System.Drawing.Color clrLBL_FColor = System.Drawing.Color.FromArgb(0, 0, 0);

//        #region "Default Unselected"
//        //Window Gradiants
//        public static System.Drawing.Color clrWinGrad_Light = System.Drawing.Color.FromArgb(255, 219, 235, 246);

//        public static System.Drawing.Color clrWinGrad_Dark = System.Drawing.Color.FromArgb(255, 180, 202, 229);
//        //Button Gradiants
//        public static System.Drawing.Color clrToolstripBtnGrad_Light = System.Drawing.Color.FromArgb(255, 31, 95, 183);

//        public static System.Drawing.Color clrToolstripBtnGrad_Dark = System.Drawing.Color.FromArgb(255, 13, 41, 79);
//        //Button Borders
//        #endregion
//        public static System.Drawing.Color clrToolstripBtnBorder = System.Drawing.Color.FromArgb(255, 133, 158, 191);

//        #region "Brushes"
//        public static System.Drawing.SolidBrush yellowHighlight = new System.Drawing.SolidBrush(highlightColor);
//        public static System.Drawing.SolidBrush BGshadowSolid = new System.Drawing.SolidBrush(clrVerBG_Shadow);
//        public static System.Drawing.SolidBrush btnBorderGoldSelectedSolid = new System.Drawing.SolidBrush(clrToolstripBtnBorder_Gold_Selected);

//        public static System.Drawing.SolidBrush btnBorderGoldPressedSolid = new System.Drawing.SolidBrush(clrToolstripBtnBorder_Gold_Pressed);
//        public static System.Drawing.SolidBrush Solid = new System.Drawing.SolidBrush(clrToolstripBtnBorder);
//        public static System.Drawing.SolidBrush TextSolidPressed_Selected = new System.Drawing.SolidBrush(clrBtn_FColorPressed_Selected);
//        public static System.Drawing.SolidBrush TextSolid = new System.Drawing.SolidBrush(clrBtn_FColor);
//        public static System.Drawing.SolidBrush OPCTextSolidPressed_Selected = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(_Opacity, clrBtn_FColorPressed_Selected));
//        #endregion
//        public static System.Drawing.SolidBrush OPCTextSolid = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(_Opacity, clrBtn_FColor));

//        #region "Pens"
//        #endregion
//        public static System.Drawing.Pen Pen = new System.Drawing.Pen(clrToolstripBtnBorder);

//        #region "Selected"
//        public static System.Drawing.Color clrToolstripBtnGrad_WhiteGold_Selected = System.Drawing.Color.FromArgb(255, 255, 245, 192);
//        public static System.Drawing.Color clrToolstripBtnGrad_Gold_Selected = System.Drawing.Color.FromArgb(255, 255, 216, 80);
//        #endregion
//        public static System.Drawing.Color clrToolstripBtnBorder_Gold_Selected = System.Drawing.Color.FromArgb(255, 194, 169, 120);

//        #region "Pressed"
//        public static System.Drawing.Color clrToolstripBtnGrad_WhiteGold_Pressed = System.Drawing.Color.FromArgb(255, 252, 194, 131);
//        public static System.Drawing.Color clrToolstripBtnGrad_Gold_Pressed = System.Drawing.Color.FromArgb(255, 249, 144, 46);
//        #endregion
//        public static System.Drawing.Color clrToolstripBtnBorder_Gold_Pressed = System.Drawing.Color.FromArgb(255, 142, 129, 101);

//        public static void SetUpThemeColors(bool blnRenderOnly)
//        {
//            if (!blnRenderOnly)
//            {
//                switch (Theme.ThemeColor)
//                {
//                    case RibbonTheme.Black:
//                        //ForeColor
//                        clrBtn_FColorPressed_Selected = System.Drawing.Color.FromArgb(0, 0, 0);
//                        clrBtn_FColor = System.Drawing.Color.FromArgb(255, 255, 255);
//                        clrLBL_FColor = System.Drawing.Color.FromArgb(0, 0, 0);

//                        //Window Gradiants
//                        clrWinGrad_Light = System.Drawing.Color.FromArgb(255, 244, 244, 244);
//                        clrWinGrad_Dark = System.Drawing.Color.FromArgb(255, 191, 191, 191);

//                        //Button Gradiants
//                        clrToolstripBtnGrad_Light = System.Drawing.Color.FromArgb(255, 122, 122, 122);
//                        clrToolstripBtnGrad_Dark = System.Drawing.Color.FromArgb(255, 71, 71, 71);

//                        //Button Border
//                        clrToolstripBtnBorder = System.Drawing.Color.FromArgb(255, 46, 46, 46);

//                        //Selected Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Selected = System.Drawing.Color.FromArgb(255, 255, 245, 192);
//                        clrToolstripBtnGrad_Gold_Selected = System.Drawing.Color.FromArgb(255, 255, 216, 80);
//                        clrToolstripBtnBorder_Gold_Selected = System.Drawing.Color.FromArgb(255, 194, 169, 120);

//                        //Pressed Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Pressed = System.Drawing.Color.FromArgb(255, 252, 194, 131);
//                        clrToolstripBtnGrad_Gold_Pressed = System.Drawing.Color.FromArgb(255, 249, 144, 46);
//                        clrToolstripBtnBorder_Gold_Pressed = System.Drawing.Color.FromArgb(255, 142, 129, 101);

//                        break;
//                    case RibbonTheme.Blue:
//                        //ForeColor
//                        clrBtn_FColorPressed_Selected = System.Drawing.Color.FromArgb(0, 25, 56);
//                        clrBtn_FColor = System.Drawing.Color.FromArgb(255, 255, 255);
//                        clrLBL_FColor = System.Drawing.Color.FromArgb(0, 0, 0);

//                        //Window Gradiants
//                        clrWinGrad_Light = System.Drawing.Color.FromArgb(255, 219, 235, 246);
//                        clrWinGrad_Dark = System.Drawing.Color.FromArgb(255, 180, 202, 229);

//                        //Button Gradiants
//                        clrToolstripBtnGrad_Light = System.Drawing.Color.FromArgb(255, 31, 95, 183);
//                        clrToolstripBtnGrad_Dark = System.Drawing.Color.FromArgb(255, 13, 41, 79);

//                        //Button Border
//                        clrToolstripBtnBorder = System.Drawing.Color.FromArgb(255, 133, 158, 191);

//                        //Selected Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Selected = System.Drawing.Color.FromArgb(255, 255, 245, 192);
//                        clrToolstripBtnGrad_Gold_Selected = System.Drawing.Color.FromArgb(255, 255, 216, 80);
//                        clrToolstripBtnBorder_Gold_Selected = System.Drawing.Color.FromArgb(255, 194, 169, 120);

//                        //Pressed Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Pressed = System.Drawing.Color.FromArgb(255, 252, 194, 131);
//                        clrToolstripBtnGrad_Gold_Pressed = System.Drawing.Color.FromArgb(255, 249, 144, 46);
//                        clrToolstripBtnBorder_Gold_Pressed = System.Drawing.Color.FromArgb(255, 142, 129, 101);

//                        break;
//                    case RibbonTheme.Green:
//                        //ForeColor
//                        clrBtn_FColorPressed_Selected = System.Drawing.Color.FromArgb(0, 25, 56);
//                        clrBtn_FColor = System.Drawing.Color.FromArgb(255, 255, 255);
//                        clrLBL_FColor = System.Drawing.Color.FromArgb(0, 0, 0);

//                        //Window Gradiants
//                        clrWinGrad_Light = System.Drawing.Color.FromArgb(255, 220, 246, 219);
//                        clrWinGrad_Dark = System.Drawing.Color.FromArgb(255, 180, 229, 182);

//                        //Button Gradiants
//                        clrToolstripBtnGrad_Light = System.Drawing.Color.FromArgb(255, 36, 206, 44);
//                        clrToolstripBtnGrad_Dark = System.Drawing.Color.FromArgb(255, 15, 79, 13);

//                        //Button Border
//                        clrToolstripBtnBorder = System.Drawing.Color.FromArgb(255, 57, 222, 65);

//                        //Selected Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Selected = System.Drawing.Color.FromArgb(255, 255, 245, 192);
//                        clrToolstripBtnGrad_Gold_Selected = System.Drawing.Color.FromArgb(255, 255, 216, 80);
//                        clrToolstripBtnBorder_Gold_Selected = System.Drawing.Color.FromArgb(255, 194, 169, 120);

//                        //Pressed Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Pressed = System.Drawing.Color.FromArgb(255, 252, 194, 131);
//                        clrToolstripBtnGrad_Gold_Pressed = System.Drawing.Color.FromArgb(255, 249, 144, 46);
//                        clrToolstripBtnBorder_Gold_Pressed = System.Drawing.Color.FromArgb(255, 142, 129, 101);

//                        break;
//                    case RibbonTheme.Purple:
//                        //ForeColor
//                        clrBtn_FColorPressed_Selected = System.Drawing.Color.FromArgb(255, 63, 6, 120);
//                        clrBtn_FColor = System.Drawing.Color.FromArgb(255, 255, 255);
//                        clrLBL_FColor = System.Drawing.Color.FromArgb(0, 0, 0);

//                        //Window Gradiants
//                        clrWinGrad_Light = System.Drawing.Color.FromArgb(255, 194, 158, 227);
//                        clrWinGrad_Dark = System.Drawing.Color.FromArgb(255, 144, 94, 188);

//                        //Button Gradiants
//                        clrToolstripBtnGrad_Light = System.Drawing.Color.FromArgb(255, 163, 133, 190);
//                        clrToolstripBtnGrad_Dark = System.Drawing.Color.FromArgb(255, 13, 41, 79);

//                        //Button Border
//                        clrToolstripBtnBorder = System.Drawing.Color.FromArgb(255, 133, 158, 191);

//                        //Selected Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Selected = System.Drawing.Color.FromArgb(255, 255, 245, 192);
//                        clrToolstripBtnGrad_Gold_Selected = System.Drawing.Color.FromArgb(255, 255, 216, 80);
//                        clrToolstripBtnBorder_Gold_Selected = System.Drawing.Color.FromArgb(255, 194, 169, 120);

//                        //Pressed Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Pressed = System.Drawing.Color.FromArgb(255, 252, 194, 131);
//                        clrToolstripBtnGrad_Gold_Pressed = System.Drawing.Color.FromArgb(255, 249, 144, 46);
//                        clrToolstripBtnBorder_Gold_Pressed = System.Drawing.Color.FromArgb(255, 142, 129, 101);

//                        break;
//                    case RibbonTheme.JellyBelly:
//                        //ForeColor
//                        clrBtn_FColorPressed_Selected = System.Drawing.Color.FromArgb(255, 235, 235, 235);
//                        clrBtn_FColor = System.Drawing.Color.FromArgb(255, 235, 235, 235);
//                        clrLBL_FColor = System.Drawing.Color.FromArgb(255, 235, 235, 235);

//                        //Window Gradiants
//                        clrWinGrad_Light = System.Drawing.Color.FromArgb(255, 72, 72, 72);
//                        clrWinGrad_Dark = System.Drawing.Color.FromArgb(255, 40, 40, 40);

//                        //Button Gradiants
//                        clrToolstripBtnGrad_Light = System.Drawing.Color.FromArgb(255, 72, 72, 72);
//                        clrToolstripBtnGrad_Dark = System.Drawing.Color.FromArgb(255, 40, 40, 40);

//                        //Button Border
//                        clrToolstripBtnBorder = System.Drawing.Color.FromArgb(255, 133, 158, 191);

//                        //Selected Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Selected = System.Drawing.Color.FromArgb(255, 48, 180, 228);
//                        clrToolstripBtnGrad_Gold_Selected = System.Drawing.Color.FromArgb(255, 48, 180, 228);
//                        clrToolstripBtnBorder_Gold_Selected = System.Drawing.Color.FromArgb(255, 58, 141, 181);

//                        //Pressed Button Gradiants
//                        clrToolstripBtnGrad_WhiteGold_Pressed = System.Drawing.Color.FromArgb(255, 11, 73, 86);
//                        clrToolstripBtnGrad_Gold_Pressed = System.Drawing.Color.FromArgb(255, 11, 73, 86);
//                        clrToolstripBtnBorder_Gold_Pressed = System.Drawing.Color.FromArgb(255, 58, 141, 181);
//                        break;
//                }
//            }
//        }
//    }
//}