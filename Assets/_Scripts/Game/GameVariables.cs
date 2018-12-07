using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameVariables : MonoBehaviour {

	public string urlToDownload = "";
	public string CSVDownloaded; //THIS IS THE RESULT.

	public Dictionary<string, int> downloadedVariables = new Dictionary<string, int>();

	/*private void Start() {
		if (urlToDownload == "") {
			Debug.Log("No URL, stopping the Load!");
			return;
		}

		StartCoroutine(CSVLoad(urlToDownload));
	}*/

	//referenced from https://www.youtube.com/watch?v=xwnL4meq-j8&feature=youtu.be by Rapid Gaming, 13 nov 2018.
	public void CSVRead() {
		StreamReader streamReader = new StreamReader(Application.dataPath + "/data.txt");
		bool endOfFile = false;

		while (!endOfFile) {
			string dataString = streamReader.ReadLine();

			if (dataString == null) {
				endOfFile = true;
				break;
			}
			string[] temp = dataString.Split(',');

			downloadedVariables.Add(temp[0], int.Parse(temp[1]));
		}

		GameController.gCont.ApplySettings();
	}

	public IEnumerator CSVLoad(string urlToDownload) {
		WWW urlRetrieved = new WWW(urlToDownload);

		yield return urlRetrieved;

		if(urlRetrieved.error == null) {
			CSVDownloaded = urlRetrieved.text;

			if (File.Exists(Application.dataPath + "/data.txt")) {
				File.Delete(Application.dataPath + "/data.txt");
			}

			File.WriteAllText(Application.dataPath + "/data.txt", urlRetrieved.text);

			CSVRead();	
		}
	}
}