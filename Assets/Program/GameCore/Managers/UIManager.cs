using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using HotLogic;
using Platform;
using UnityEngine;
using Utils;
using DG.Tweening;

/// <summary>
/// UI视图结构体
/// </summary>
public struct UIViewStruct
{
    /// <summary>
    /// Prefab Path
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// UI类型
    /// </summary>
    public EUIState State { get; set; }
    /// <summary>
    /// 父节点
    /// </summary>
    public Transform Parent { get; set; }
    /// <summary>
    /// Prefab GameObject
    /// </summary>
    public GameObject Target { get; set; }
    /// <summary>
    /// 模块名字
    /// </summary>
    public string ModuleName { get; set; }
}

public class UIManager : TMonoSingleton<UIManager>
{
    /// <summary>
    /// 功能UI第一个子节点名称 用于Tween动画播放
    /// </summary>
    const string TweenRootName = "TweenRoot";
    /// <summary>
    /// 功能UI第二个子节点名称 用于控制屏幕高斯模糊
    /// </summary>
    const string EffectCapturedImage = "EffectCapturedImage";
    //开启动画时间
    const float TweenOpenTime = .4f;
    //关闭动画时间
    const float TweenCloseTime = .3f;
    /// <summary>
    /// 功能UI 栈存储
    /// </summary>
    //private Stack<UIViewStruct> _stacks = new Stack<UIViewStruct>();
    private Dictionary<EUIState, Stack<UIViewStruct>> _stacksDic = new Dictionary<EUIState, Stack<UIViewStruct>>();

    public override void Awake()
    {
        Instance = this;
    }
    
    /// <summary>
    /// 通用打开UI模块
    /// </summary>
    /// <param name="path">Prefab路径</param>
    /// <param name="moduleName">功能类型</param>
    /// <param name="state">UI状态</param>
    /// <param name="tweenComplete">动画完成回调</param>
    /// <param name="lastActive">上一个UI Active</param>
    /// <param name="isAddCommonTween">是否播放动画</param>
    /// <param name="isEffct">高斯模糊标记</param>
    /// <returns></returns>
    public void OpenCommonUI(string path, string moduleName, EUIState state, Action<GameObject> callBack, Action tweenPlayComplete, bool lastActive = false, bool isAddCommonTween = false, bool isEffct = false)
    {
        #region 声音控制
        //待声音统一管理器实现播放
        #endregion
        if (IsContinasPrefabPathInStack(path, state))
        {
            GameObject go = GetGameObjectByPathInStack(path, state);
            go.transform.SetAsLastSibling();
            UIAbstractViewObject uView = go.GetOrAddComponent<UIAbstractViewObject>();
            if (uView != null)
            {
                uView.UINodeState = state;
                uView.PrefabPath = path;
            }
            callBack?.Invoke(go);
        }
        else
        {
            if (GetUIStack(state).Count > 0)
            {
                GetUIStack(state).Peek().Target.SetActive(lastActive); //打开新UI  设置上一个UI是否隐藏
            }
            //获取UIPrefab的实例
            GameObject prefab = null;
            libx.AssetRequest _request = null;
            ABResources.LoadRes<GameObject>(path, (a, request) =>
            {
                prefab = a;
                _request = request;
            }, ABResources.MatchMode.Prefab); //ResourcesManager.Instance.LoadAsset<GameObject>(path);
            Transform parent = GetParentTransform(state);
            GameObject go = UITools.AddChild(parent, Instantiate(prefab));
            _request.Require(go);
            //add canvas start
            Canvas cas = go.GetComponent<Canvas>();
            if (cas == null) { cas = go.AddComponent<Canvas>(); }
            //cas.overridePixelPerfect  = inherit;
            cas.overridePixelPerfect = false;
            cas.pixelPerfect = false;
            cas.overrideSorting = true;
            cas.sortingOrder = GetOrder(state);
            cas.additionalShaderChannels = AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent | AdditionalCanvasShaderChannels.TexCoord1;
            UnityEngine.UI.GraphicRaycaster gr = go.GetComponent<UnityEngine.UI.GraphicRaycaster>();
            if (gr == null) { gr = go.AddComponent<UnityEngine.UI.GraphicRaycaster>(); }
            gr.ignoreReversedGraphics = false;
            gr.blockingObjects = UnityEngine.UI.GraphicRaycaster.BlockingObjects.None;
            //add canvas end
            UIAbstractViewObject uView = go.GetOrAddComponent<UIAbstractViewObject>();

            uView.UINodeState = state;
            uView.PrefabPath = path;


            go.transform.SetAsLastSibling();
            GetUIStack(state).Push(new UIViewStruct { Path = path, State = state, Parent = parent, Target = go, ModuleName = moduleName });

            if (isAddCommonTween)
            {
                Transform tf = UnityUtilTools.GetSingleObject<Transform>(go, TweenRootName);
                if (tf != null)
                {
                    tf.localScale = Vector3.zero; //若开启高斯模糊 则UI需要等待1帧
                    tf.DOScale(1, TweenOpenTime).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        tweenPlayComplete();
                    }).Play();
                }
                else
                {
                    tweenPlayComplete();
                }
            }
            else
            {
                tweenPlayComplete();
            }
            callBack?.Invoke(go);
        }
    }
    public void OpenCommonUIAsync(string path, string moduleName, EUIState state, Action<GameObject> callBack, Action tweenPlayComplete, bool lastActive = false, bool isAddCommonTween = false, bool isEffct = false)
    {
        if (IsContinasPrefabPathInStack(path, state))
        {
            GameObject go = GetGameObjectByPathInStack(path, state);
            go.transform.SetAsLastSibling();
            UIAbstractViewObject uView = go.GetOrAddComponent<UIAbstractViewObject>();
            if (uView != null)
            {
                uView.UINodeState = state;
                uView.PrefabPath = path;
            }
            callBack?.Invoke(go);
        }
        else
        {
            if (GetUIStack(state).Count > 0)
            {
                GetUIStack(state).Peek().Target.SetActive(lastActive); //打开新UI  设置上一个UI是否隐藏
            }
            //获取UIPrefab的实例
            ABResources.LoadResAsync<GameObject>(path, (a, request) =>
            {
                Transform parent = GetParentTransform(state);
                GameObject go = UITools.AddChild(parent, Instantiate(a));
                request.Require(go);
                //add canvas start
                Canvas cas = go.GetComponent<Canvas>();
                if (cas == null) { cas = go.AddComponent<Canvas>(); }
                //cas.overridePixelPerfect  = inherit;
                cas.overridePixelPerfect = false;
                cas.pixelPerfect = false;
                cas.overrideSorting = true;
                cas.sortingOrder = GetOrder(state);
                cas.additionalShaderChannels = AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent | AdditionalCanvasShaderChannels.TexCoord1;
                UnityEngine.UI.GraphicRaycaster gr = go.GetComponent<UnityEngine.UI.GraphicRaycaster>();
                if (gr == null) { gr = go.AddComponent<UnityEngine.UI.GraphicRaycaster>(); }
                gr.ignoreReversedGraphics = false;
                gr.blockingObjects = UnityEngine.UI.GraphicRaycaster.BlockingObjects.None;
                //add canvas end
                UIAbstractViewObject uView = go.GetOrAddComponent<UIAbstractViewObject>();


                uView.UINodeState = state;
                uView.PrefabPath = path;


                go.transform.SetAsLastSibling();
                GetUIStack(state).Push(new UIViewStruct { Path = path, State = state, Parent = parent, Target = go, ModuleName = moduleName });

                if (isAddCommonTween)
                {
                    Transform tf = UnityUtilTools.GetSingleObject<Transform>(go, TweenRootName);
                    if (tf != null)
                    {
                        tf.localScale = Vector3.zero; //若开启高斯模糊 则UI需要等待1帧
                        tf.DOScale(1, TweenOpenTime).SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            tweenPlayComplete();
                        }).Play();
                    }
                    else
                    {
                        tweenPlayComplete();
                    }
                }
                else
                {
                    tweenPlayComplete();
                }
                callBack?.Invoke(go);
            }, ABResources.MatchMode.Prefab);
        }
    }
    /// <summary>
    /// 检测指定的prefab路径是否已经在stack中
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool IsContinasPrefabPathInStack(string path, EUIState state)
    {
        foreach (var item in GetUIStack(state))
        {
            if (item.Path == path) { return true; }
        }
        return false;
    }
    /// <summary>
    /// 根据指定路径获取
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private GameObject GetGameObjectByPathInStack(string path, EUIState state)
    {
        foreach (var item in GetUIStack(state))
        {
            if (item.Path == path)
            {
                return item.Target;
            }
        }
        return null;
    }
    /// <summary>
    /// 根据UI节点的类型获取节点的Transform
    /// </summary>
    /// <param name="state"></param>·
    /// <returns></returns>
    Transform GetParentTransform(EUIState state)
    {
        switch (state)
        {
            case EUIState.PreLoadRoot:
                return UIRoot.Instance.PreLoadRoot;
            case EUIState.DisplayLoadingRoot:
                return UIRoot.Instance.DisplayLoadingRoot;
            case EUIState.Dynamic:
                return UIRoot.Instance.DynamicRoot;
            case EUIState.GlobaRoot:
                return UIRoot.Instance.GlobalsRoot;
            case EUIState.Notice:
                return UIRoot.Instance.NoticeRoot;
            case EUIState.TopSideRoot:
                return UIRoot.Instance.TopSideRoot;
        }
        return null;
    }
    /// <summary>
    /// 获取OrderId
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    int GetOrder(EUIState state)
    {
        return GetBaseOrder(state) + GetUINumBySameState(state) * UIRoot.SpacingOrder;
    }
    /// <summary>
    /// 根据UI的层级类型获取本层级的基础Order
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    int GetBaseOrder(EUIState state)
    {
        switch (state)
        {
            case EUIState.PreLoadRoot:
            case EUIState.Dynamic:
                return UIRoot.DynamicOrder;
            case EUIState.GlobaRoot:
                return UIRoot.GlobalsOrder;
            case EUIState.DisplayLoadingRoot:
                return UIRoot.DisplayOrder;
            case EUIState.Notice:
                return UIRoot.NoticeOrder;
            case EUIState.TopSideRoot:
                return UIRoot.TopSideOrder;
        }
        return UIRoot.DynamicOrder;
    }
    /// <summary>
    /// 获取同层级下UI数量
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    int GetUINumBySameState(EUIState state)
    {
        int num = 0;
        var stacks = GetUIStack(state);
        foreach (var item in stacks)
        {
            if (item.State == state)
            {
                num++;
            }
        }
        return num;
    }
    /// <summary>
    /// 关闭最后一次打开的UI
    /// </summary>
    /// <param name="enableAnimation">是否开启关闭动画</param>
    public void CloseLastUI(bool enableAnimation = false, EUIState state = EUIState.Dynamic)
    {
        var stacks = GetUIStack(state);
        if (stacks.Count > 0)   //通用Tips + 主UI不关闭
        {
            //add tween
            if (enableAnimation)
            {
                UIViewStruct vs = stacks.Pop();
                Transform tf = vs.Target.transform.GetChild(0);
                //确保Prefab第一个子节点名称一致,用于动画控制。背景遮罩不参与动画控制
                if (tf != null && tf.name == TweenRootName)
                {
                    //可选择不同曲线 或创建AnimationCurve 
                    tf.DOScale(0, TweenCloseTime).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        TweenCompletedRemove(vs, state);
                    }).Play();
                }
                else
                {
                    TweenCompletedRemove(vs, state);
                }
            }
            else
            {
                UIViewStruct ps = stacks.Pop();
                TweenCompletedRemove(ps, state);
            }
        }
    }
    /// <summary>
    /// 当动画完成后进行真实的UI移除处理
    /// </summary>
    /// <param name="vs">需要移除的UI结构体对象</param>
    void TweenCompletedRemove(UIViewStruct vs, EUIState state)
    {
        RemoveHotUpdateModule(vs.ModuleName);
        var stacks = GetUIStack(state);
        if (stacks.Count > 0) //此处可先显示主要UI  动画仅控制自身
        {
            UIViewStruct target = stacks.Peek();
            if (target.Target.activeSelf)
            {
                //target.Target.SetActive(false);
                //target.Target.SetActive(true);
            }
            else
            {
                target.Target.SetActive(true);
            }
        }
        //立即销毁
        DestroyImmediate(vs.Target.gameObject);
    }
    /// <summary>
    /// 移除功能模块
    /// </summary>
    /// <param name="moduleName"></param>
    void RemoveHotUpdateModule(string moduleName)
    { 
        GameLogicManager.Instance.RemoveModule(moduleName);
    }

    /// <summary>
    /// 关闭所有打开的UI
    /// </summary>
    public void CloseAllUI(EUIState state = EUIState.Dynamic)
    {
        var stacks = GetUIStack(state);
        while (stacks.Count > 1)  //默认主UI不被关闭 当进入游戏内部后主UI在栈的最下面一个
        {
            UIViewStruct ps = stacks.Pop();
            RemoveHotUpdateModule(ps.ModuleName);
            Destroy(ps.Target);
        }
        if (stacks.Count > 0)
        {
            //激活栈底的最后一个UI面板
            UIViewStruct us = stacks.Peek();
            us.Target.SetActive(true);
        }
    }
    /// <summary>
    /// 关闭在指定模块之后的所有UI
    /// </summary>
    /// <param name="moduleName">指定的模块名</param>
    public void CloseAllUIAfterThis(string moduleName, EUIState state = EUIState.Dynamic)
    {
        List<string> moduleNameList = new List<string>();
        var stacks = GetUIStack(state);
        foreach (var item in stacks)
        {
            moduleNameList.Add(item.ModuleName);
        }
        if (moduleNameList.Contains(moduleName))
        {
            while (!IsLastModule(moduleName, state))
            {
                CloseLastUI(false, state);
            }
        }
    }
    /// <summary>
    /// 是否最后显示模块
    /// </summary>
    /// <param name="moduleName">指定的模块名</param>
    /// <returns></returns>
    public bool IsLastModule(string moduleName, EUIState state = EUIState.Dynamic)
    {
        var stacks = GetUIStack(state);
        if (stacks.Count > 0)
        {
            return stacks.Peek().ModuleName == moduleName;
        }
        return false;
    }

    public GameObject GetObjectObj(String path,Transform parent)
    {
        GameObject prefab = null;
        libx.AssetRequest _request = null; 
        ABResources.LoadRes<GameObject>(path, (a, request) =>
        {
            prefab = a;
            _request = request;
        }, ABResources.MatchMode.Prefab); //ResourcesManager.Instance.LoadAsset<GameObject>(path);

        GameObject go = UITools.AddChild(parent, Instantiate(prefab));
        _request.Require(go);
        go.gameObject.SetActive(true);
        return go;
    }
    
    Stack<UIViewStruct> GetUIStack(EUIState state = EUIState.Dynamic)
    {
        if (_stacksDic.ContainsKey(state))
        {
            return _stacksDic[state];
        }
        else
        {
            Stack<UIViewStruct> stack = new Stack<UIViewStruct>();
            _stacksDic.Add(state, stack);
            return stack;
        }
    }

    #region 设置特效渲染层级
    /// <summary>
    /// 动态功能节点下的特效设置  渲染层默认+1
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="uView"></param>
    /// <param name="isUI">True UI需要添加Canvas 其余使用Render控制</param>
    /// <param name="addOrder"></param>
    public void SetEffectOrder(GameObject effect, UIAbstractViewObject uView, bool isUI = false, int addOrder = 1)
    {
        int order = UIRoot.DynamicOrder;
        if (uView != null)
        {
            Canvas cas = uView.GetComponent<Canvas>();
            if (cas != null)
            {
                order = cas.sortingOrder + addOrder;
            }
            else
            {
                order = GetEffectBaseOrder(uView.UINodeState, uView.PrefabPath) + addOrder;
            }
        }

        if (effect != null)
        {
            if (isUI)
            {
                var cas = effect.GetComponent<Canvas>();
                if (cas == null) { cas = effect.AddComponent<Canvas>(); }
                cas.overridePixelPerfect = false;
                cas.pixelPerfect = false;
                cas.overrideSorting = true;
                cas.sortingOrder = order;
                cas.additionalShaderChannels = AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent | AdditionalCanvasShaderChannels.TexCoord1;
                var gr = effect.GetComponent<UnityEngine.UI.GraphicRaycaster>();
                if (gr == null) { gr = effect.AddComponent<UnityEngine.UI.GraphicRaycaster>(); }
                gr.ignoreReversedGraphics = false;
                gr.blockingObjects = UnityEngine.UI.GraphicRaycaster.BlockingObjects.None;
            }
            else
            {
                //将特效统一设置缩放系数
                //effect.transform.localScale = new Vector3(100, 100, 100);
                // var scale = UIRoot.Instance.GetScreenXRatio;
                // foreach (var item in effect.GetComponentsInChildren<Renderer>())
                // {
                //     if (item != null)
                //     {
                //         item.sortingOrder = order;
                //         var ps = item.GetComponent<ParticleSystem>();
                //         if (ps != null)
                //         {
                //             ps.transform.localScale = new Vector3(ps.transform.localScale.x * scale, ps.transform.localScale.y * scale, ps.transform.localScale.z * scale);
                //         }
                //     }
                // }
            }
        }
    }
    /// <summary>
    /// 获取指定特效在给定的节点的层级Order
    /// </summary>
    /// <param name="state"></param>
    /// <param name="prefabPath"></param>
    /// <returns></returns>
    int GetEffectBaseOrder(EUIState state, string prefabPath)
    {
        var baseOrder = GetBaseOrder(state);
        var addOrder = 0;

        List<UIViewStruct> viewStructs = new List<UIViewStruct>();
        var stacks = GetUIStack(state);
        foreach (var item in stacks)
        {
            if (item.State == state)
            {
                viewStructs.Add(item);
            }
        }
        for (var i = 0; i < viewStructs.Count; i++)
        {
            if (viewStructs[i].Path == prefabPath)
            {
                addOrder = i * UIRoot.SpacingOrder;
                break;
            }
        }
        return baseOrder + addOrder;
    }

    /// <summary>
    /// 指定层级Order设置
    /// </summary>
    /// <param name="state"></param>
    /// <param name="addOrder">增加的Order</param>
    public void SetEffectOrder(GameObject effect, EUIState state, int addOrder = 2)
    {
        int order = GetBaseOrder(state) + addOrder;
        if (effect != null)
        {
            //将特效统一设置缩放系数
            //effect.transform.localScale = new Vector3(100, 100, 100);
            // float scale = UIRoot.Instance.GetScreenXRatio;
            // foreach (var item in effect.GetComponentsInChildren<Renderer>())
            // {
            //     if (item != null)
            //     {
            //         item.sortingOrder = order;
            //         ParticleSystem ps = item.GetComponent<ParticleSystem>();
            //         if (ps != null)
            //         {
            //             ps.transform.localScale = new Vector3(ps.transform.localScale.x * scale, ps.transform.localScale.y * scale, ps.transform.localScale.z * scale);
            //         }
            //     }
            // }
        }
    }
    #endregion
}
