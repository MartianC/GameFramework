using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameCore;

namespace HotLogic
{
    public class LoginModuleView : BaseView
    {        
        #region Key
        
        private const string KC_Title_TXT = "KC_Title_TXT";
        private const string KC_Login_BTN = "KC_Login_BTN";
        private const string KC_Regist_BTN = "KC_Regist_BTN";


        #endregion

        #region Component
        
        public Text C_Title_TXT { get; private set; }
        public Button C_Login_BTN { get; private set; }
        public Button C_Regist_BTN { get; private set; }


        #endregion
                
        public override void GetObjects(UIAbstractViewObject mScript)
        {
            base.GetObjects(mScript);
            var objName = new HashSet<string>
            {
                KC_Title_TXT,
                KC_Login_BTN,
                KC_Regist_BTN,
            };
            _ObjectDict = mScript.FindGameObject(objName);

            C_Title_TXT = GetObject<Text>(KC_Title_TXT);
            C_Login_BTN = GetObject<Button>(KC_Login_BTN);
            C_Regist_BTN = GetObject<Button>(KC_Regist_BTN);

        }
    }
}