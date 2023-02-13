using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Oknan
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Unity Royale/Card Data")]
    public class CardData : ScriptableObject
    {

        [Header("Card graphics")]
        public Sprite cardImage;

        public string imageUrl;

        public IEnumerator LoadSpriteFromWeb()
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to download image: " + www.error);
                    yield break;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                cardImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
        }




        [Header("List of Placeables")]
        public PlaceableData[] placeablesData; //link to all the Placeables that this card spawns
        public Vector3[] relativeOffsets; //the relative offsets (from cursor) where the placeables will be dropped
    }
}