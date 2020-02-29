using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class Ads : MonoBehaviour
{
	string gameId = "3415107";
	bool testMode = true;
	string placementId = "StandardPlaying";
	// Start is called before the first frame update
	void Start()
    {
		Advertisement.Initialize(gameId, testMode);
		StartCoroutine(ShowBannerWhenReady());
		
	}

	IEnumerator ShowBannerWhenReady()
	{
		while (!Advertisement.IsReady(placementId))
		{
			yield return new WaitForSeconds(0.5f);
		}
		Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
		
		Advertisement.Banner.Show(placementId);
		
	}
}


/*bannerHeight = Camera.main.ScreenToWorldPoint(new Vector3(1, 50 * Screen.dpi / 160, 1)).y - Camera.main.ScreenToWorldPoint(new Vector3(1, 0, 1)).y;
 
public float GetBannerHeight()
{
	return Mathf.RoundToInt(50 * Screen.dpi / 160);
}
	 */
