using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHTTP;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	private Text errorText;
	private Text successText;
	private string validURL = "https://jsonplaceholder.typicode.com/posts/";
	private string invalidURL = "https://jsonplaceholder.net/articles/";

	[Space]

	public string AllPlayerDataURL;
	public string IndividualPlayerDataURL;
	public string TopTenPlayerDataURL;
	public string PostPlayerDataURL;

 //   [Space]

	//public string Address;
	//public int Lives;
	//public int Player;

	[Space]

	private PlayerVariables playerVariables;

	void Start () {		
		playerVariables = GetComponent<PlayerData>().playerVariables;
	}

	IEnumerator Get(string baseUrl, int postId) {
		Request request = new Request (baseUrl);

		Client http = new Client ();
		yield return http.Send(request);
		ProcessResult(http);
	}

	IEnumerator Post() {
		
		PlayerDataBody body = APIManager.Instance.playerData;

		Request request = new Request (PostPlayerDataURL).Post(RequestBody.From (body));

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	IEnumerator PostWithFormData() {
		
		FormData formData = new FormData().AddField("Test", "5");

		Request request = new Request (PostPlayerDataURL)
			.Post (RequestBody.From(formData));

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	//for updating values on server
	IEnumerator Put() {
		
        PlayerDataBody body = APIManager.Instance.playerData;

        Request request = new Request(PostPlayerDataURL)
            .Put(RequestBody.From<PlayerDataBody>(body));        

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	IEnumerator Delete() {
		Request request = new Request (validURL + "1")
			.Delete ();

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	IEnumerator ClearOutput() {
		yield return new WaitForSeconds (2f);
		//errorText.text = "";
		//successText.text = "";
	}

	void ProcessResult(Client http) {
		if (http.IsSuccessful ()) {
			Response resp = http.Response ();
			//successText.text = "status: " + resp.Status().ToString() + "\nbody: " + resp.Body();
			Debug.Log("status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
		} else {
			//errorText.text = "error: " + http.Error();
		}
		StopCoroutine (ClearOutput ());
		StartCoroutine (ClearOutput ());
	}

    #region Update values on server
	public void UpdateValuesOnServer()
    {   
        StartCoroutine(Put());

		//UpdatePost();
    }

    #endregion

    public void GetPost() {
		//StartCoroutine (Get (validURL, 1));
		StartCoroutine (Get (IndividualPlayerDataURL, 1));
	}

	public void GetTopTenPlayerData()
    {
		StartCoroutine(Get(TopTenPlayerDataURL, 1));
	}

	public void GetIndividualPlayerData()
    {
		StartCoroutine(Get(IndividualPlayerDataURL, 1));
	}

	public void GetAllPlayerData()
    {
		StartCoroutine(Get(AllPlayerDataURL, 1));
	}

	public void UpdatePlayer()
    {
		StartCoroutine(PostWithFormData());
	}

	public void CreatePost() {
		StartCoroutine (Post ());
	}

	public void UpdatePost() {
		StartCoroutine (Put ());
	}

	public void DeletePost() {
		StartCoroutine (Delete ());
	}

	public void GetNonExistentPost() {
		StartCoroutine (Get (validURL, 999));
	}

	public void GetInvalidUrl() {
		StartCoroutine (Get (invalidURL, 1));
	}

	public void CreatePostWithFormData() {
		StartCoroutine (PostWithFormData ());
	}
}
