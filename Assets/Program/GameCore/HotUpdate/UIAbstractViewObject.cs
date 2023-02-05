namespace GameCore
{
     public class UIAbstractViewObject : UIViewObjectBase
     {
          /// <summary>
          /// UI节点类型
          /// </summary>
          public EUIState UINodeState = EUIState.Dynamic;
          /// <summary>
          /// Prefab路径
          /// </summary>
          public string PrefabPath = string.Empty;

     }
}