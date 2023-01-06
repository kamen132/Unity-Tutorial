namespace DefaultNamespace
{
    public class GameManager : MonoSingleton<GameManager>
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            DataBaseManager.Instance.Launch();
        }
    }
}