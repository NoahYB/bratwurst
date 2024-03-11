using UnityEditor;
using UnityEngine;
 
public class TextureConverter : MonoBehaviour
{
    public Shader materialShader;
    public string extension = "_material";
 
    [MenuItem("Assets/Convert Textures")]
    static void ConvertTextures()
    {
        TextureConverter settings = FindObjectOfType<TextureConverter>();
 
        if (settings?.materialShader != null) {
 
            foreach (string guid in AssetDatabase.FindAssets("t:texture", new string[] { "Assets" })) {
 
                string texturePath = AssetDatabase.GUIDToAssetPath(guid);
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
 
                Material newMaterial = new Material(settings.materialShader) {
                    name = texture.name,
                    mainTexture = texture,
                };
 
                string materialPath = settings.GetFolderPath(texturePath) + newMaterial.name + settings.extension + ".mat";
                AssetDatabase.CreateAsset(newMaterial, materialPath);
 
                Debug.Log(string.Format("Created material {0} at {1}", newMaterial.name, materialPath));
            }
 
            Destroy(settings.gameObject);
        }
    }
    private string GetFolderPath(string filePath)
    {
        string[] segments = filePath.Split('/');
        string result = string.Empty;
 
        for (int i = 0; i < segments.Length - 1; i++)
            result += segments[i] + '/';
 
        return result;
    }
}
