using System.Threading.Tasks;
using Project.Dev.Infrastructure.AssetManager;
using Project.Dev.Infrastructure.Factories.Interfaces;
using Project.Dev.Meta.UI.HudController;
using Project.Dev.Meta.UI.MenuController;
using UnityEngine;
using Zenject;

namespace Project.Dev.Infrastructure.Factories
{
    public class UIFactorie : IUIFactorie
    {
        private const string MenuPrefabId = "MenuPrefab";
        private const string HUDPrefabId = "HudPrefab";
        private const string RootUiPrefabId = "UIRootPrefab";

        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;

        private Canvas _uiRoot;

        public UIFactorie(DiContainer container, IAssetProvider assetProvider)
        {
            _container = container;
            _assetProvider = assetProvider;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(key: RootUiPrefabId);
            await _assetProvider.Load<GameObject>(key: HUDPrefabId);
            await _assetProvider.Load<GameObject>(key: MenuPrefabId);
        }

        public void CleanUp()
        {
             _assetProvider.Release(key: MenuPrefabId);
        }

        public async Task CreateUiRoot()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: RootUiPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<MenuController> CreateMenu()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: MenuPrefabId);
            var menu = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<MenuController>();
            _container.InjectGameObject(menu.gameObject);
            return menu;
        }

        public async Task<HudController> CreateHud()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: HUDPrefabId);
            var hud = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<HudController>();
            _container.Inject(hud);
            return hud;
        }
    }
}
