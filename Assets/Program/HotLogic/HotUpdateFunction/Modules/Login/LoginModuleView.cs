using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace HotLogic
{
    public class LoginModuleView: BaseView
    {
        #region Key=>Component name
        const string GL_btnLogin = "GL_btnLogin";
        const string GL_btnRegist = "GL_btnRegist";
        #endregion
        
        #region
        public Button _btnLogin;
        public Button _btnRegist;
        #endregion

        public override void GetObjects(UIAbstractViewObject mScript)
        {
            base.GetObjects(mScript);
            List<string> objName = new List<string>
            {
                GL_btnLogin,
                GL_btnRegist,
            };
            _ObjectDict = mScript.FindGameObject(objName);


            _btnLogin = GetObject<Button>(GL_btnLogin);
            _btnRegist = GetObject<Button>(GL_btnRegist);
        }

    }
}