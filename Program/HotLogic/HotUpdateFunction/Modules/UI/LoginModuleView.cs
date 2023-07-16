using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace HotLogic
{
    public class #NAME# : BaseView
    {        
        #region Key
        


        #endregion

        #region Component
        


        #endregion
        
        public List<FMCommonText> LanguageList;
        
        public override void GetObjects(UIAbstractViewObject mScript)
        {
            base.GetObjects(mScript);
            var objName = new HashSet<string>
            {
##
            };
            _ObjectDict = mScript.FindGameObject(objName, out LanguageList);

        }
    }
}