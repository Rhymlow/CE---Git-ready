using GoogleMobileAds.Api;
using UnityEngine;

public class AddManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    [SerializeField] private string adUnitId = "ca-app-pub-3940256099942544/5224354917";
    
    void Start()
    {
        GameSystem.addManager = this;
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob inicializado");
            LoadRewardedAd();
        });
    }

    public void LoadRewardedAd()
    {
        AdRequest adRequest = new AdRequest();

        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Error al cargar el anuncio: " + error);
                return;
            }

            rewardedAd = ad;

            // Asigna el callback para cuando el anuncio se cierra
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Anuncio cerrado");
                LoadRewardedAd(); // Opcional: recarga el anuncio automáticamente
            };

            // Recompensa
            rewardedAd.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log($"Anuncio pagado: {adValue.Value} {adValue.CurrencyCode}");
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError err) =>
            {
                Debug.LogError("Error al mostrar el anuncio: " + err);
            };
        });
    }

    //Use this function wherever you want to display a rewarded.
    public void ShowAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"Jugador ganó recompensa: {reward.Type} ({reward.Amount})          " + Time.timeScale);
                Time.timeScale = 1;
                // Aquí entregas la recompensa al jugador
            });
        }
        else
        {
            Debug.Log("El anuncio no está listo.");
        }
    }
}
