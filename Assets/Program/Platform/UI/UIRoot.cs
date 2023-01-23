using UnityEngine;
using UnityEngine.UI;

namespace Platform
{
    public class UIRoot : TMonoSingleton<UIRoot>
    {
        /// <summary>
        /// UI的固定层级的固定挂点
        /// </summary>
        public Transform PreLoadRoot;

        public Camera UICamera;
        public Transform DynamicRoot;
        public Transform GlobalsRoot;
        public Transform DisplayLoadingRoot;
        public Transform GuideFirstRoot;
        public Transform GuideSecondRoot;
        public Transform NoticeRoot;
        public Transform TopSideRoot;

        /// <summary>
        /// 屏幕的分辨率
        /// </summary>
        private Vector2 mResolution = new Vector2(1334, 750);

        /// <summary>
        /// UI各固定挂点层级设置
        /// </summary>
        public const int DynamicOrder = 0;

        public const int GlobalsOrder = 200;
        public const int DisplayOrder = 400;
        public const int NoticeOrder = 600;
        public const int TopSideOrder = 800;

        /// <summary>
        /// 各层级下间隔Order 预留特效Order
        /// </summary>
        public const int SpacingOrder = 10;

        public override void Awake()
        {
            Instance = this;

            UICamera = GetComponentInChildren<Camera>();
            
            #region 自动处理补全各个指定的固定挂点

            if (PreLoadRoot == null)
            {
                PreLoadRoot = transform.Find("PreLoadRoot");
                if (PreLoadRoot == null)
                {
                    PreLoadRoot = UITools.AddChild(gameObject.transform, new GameObject("PreLoadRoot")).transform;
                    PreLoadRoot.gameObject.SetActive(false);
                }
            }

            if (DynamicRoot == null)
            {
                DynamicRoot = transform.Find("DynamicPanel");
                if (DynamicRoot == null)
                {
                    DynamicRoot = UITools.AddChild(gameObject.transform, new GameObject("DynamicPanel")).transform;
                }
            }

            if (GlobalsRoot == null)
            {
                GlobalsRoot = transform.Find("GlobalsRoot");
                if (GlobalsRoot == null)
                {
                    GlobalsRoot = UITools.AddChild(gameObject.transform, new GameObject("GlobalsRoot")).transform;
                }
            }

            if (NoticeRoot == null)
            {
                NoticeRoot = transform.Find("NoticeRoot");
                if (NoticeRoot == null)
                {
                    NoticeRoot = UITools.AddChild(gameObject.transform, new GameObject("NoticeRoot")).transform;
                }
            }

            if (DisplayLoadingRoot == null)
            {
                DisplayLoadingRoot = transform.Find("DisplayLoadingRoot");
                if (DisplayLoadingRoot == null)
                {
                    DisplayLoadingRoot = UITools.AddChild(gameObject.transform, new GameObject("DisplayLoadingRoot")).transform;
                }
            }

            if (GuideFirstRoot == null)
            {
                GuideFirstRoot = transform.Find("GuideFirstRoot");
                if (GuideFirstRoot == null)
                {
                    GuideFirstRoot = UITools.AddChild(gameObject.transform, new GameObject("GuideFirstRoot")).transform;
                }
            }

            if (GuideSecondRoot == null)
            {
                GuideSecondRoot = transform.Find("GuideSecondRoot");
                if (GuideSecondRoot == null)
                {
                    GuideSecondRoot = UITools.AddChild(gameObject.transform, new GameObject("GuideSecondRoot")).transform;
                }
            }

            if (TopSideRoot == null)
            {
                TopSideRoot = transform.Find("TopSideRoot");
                if (TopSideRoot == null)
                {
                    TopSideRoot = UITools.AddChild(gameObject.transform, new GameObject("TopSideRoot")).transform;
                }
            }

            #endregion
        }

        public void Start()
        {
            //获取屏幕的分辨率
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                mResolution = scaler.referenceResolution;
            }
        }
    }
}