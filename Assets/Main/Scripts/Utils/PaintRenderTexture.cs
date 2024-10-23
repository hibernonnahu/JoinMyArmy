using System.IO;
using UnityEngine;

public class PaintRenderTexture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture renderTexture; // El RenderTexture que vas a pintar
    public Color paintColor = Color.red; // El color para pintar (rojo por defecto)
    public float brushRadius = 10f; // Radio del área de pintura en píxeles
    [Range(0f, 1f)] public float brushHardness = 0.5f; // Dureza del pincel (0 = suave, 1 = duro)
    float tick=0;
    private void Update()
    {
        // Verifica si el botón izquierdo del mouse fue presionado
        if (Input.GetMouseButton(0))
        {
            tick -= Time.deltaTime;
            if (tick < 0)
            {
                tick += 0.2f;
                PaintTextureAtMousePosition(cam);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            paintColor = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            paintColor = Color.green;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            paintColor = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PaintRed();
        }

    }
    public void PaintRed()
    {
        // Creamos un nuevo RenderTexture en el que pintaremos
        RenderTexture.active = renderTexture;

        // Crea un nuevo texture y lo pinta de rojo
        Texture2D tempTexture = new Texture2D(renderTexture.width, renderTexture.height);
        Color[] colors = new Color[renderTexture.width * renderTexture.height];

        // Rellenamos el array con el color rojo
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red; // Cambia el color a rojo
        }

        // Asignamos los colores a la textura temporal
        tempTexture.SetPixels(colors);
        tempTexture.Apply();

        // Copiamos la textura al RenderTexture
        Graphics.Blit(tempTexture, renderTexture);

        // Liberamos la textura temporal
        Destroy(tempTexture);

        // Reiniciamos el RenderTexture activo
        RenderTexture.active = null;
    }
    

    private void PaintTextureAtMousePosition(Camera camera)
    {
        // Obtén la posición del mouse en pantalla y conviértela a coordenadas de UV
        Vector2 mousePos = Input.mousePosition;

        // Convertir la posición del mouse a coordenadas del mundo
        Vector3 worldPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));

        // Convertir las coordenadas del mundo a coordenadas de UV del RenderTexture
        float u = (worldPos.x* camera.orthographicSize /100+ (camera.orthographicSize * camera.aspect)) / (camera.orthographicSize * 2 * camera.aspect);
        
        float v = (worldPos.z*camera.orthographicSize / 100 + camera.orthographicSize) / (camera.orthographicSize * 2);

        // Convierte las coordenadas UV a coordenadas de píxeles en el RenderTexture
        int pixelX = Mathf.Clamp((int)(u * renderTexture.width), 0, renderTexture.width - 1);
        int pixelY = Mathf.Clamp((int)(v * renderTexture.height), 0, renderTexture.height - 1);

        // Crea una textura temporal para pintar
        Texture2D tempTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Copia los datos del RenderTexture a la textura temporal
        RenderTexture.active = renderTexture;
        tempTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTexture.Apply();

        // Pinta un área circular en torno a la posición del mouse
        for (int y = -Mathf.CeilToInt(brushRadius); y <= Mathf.CeilToInt(brushRadius); y++)
        {
            for (int x = -Mathf.CeilToInt(brushRadius); x <= Mathf.CeilToInt(brushRadius); x++)
            {
                int posX = pixelX + x;
                int posY = pixelY + y;

                // Verifica si el píxel está dentro del radio y dentro de los límites de la textura
                if (posX >= 0 && posX < renderTexture.width && posY >= 0 && posY < renderTexture.height)
                {
                    float distance = Mathf.Sqrt(x * x + y * y);
                    if (distance <= brushRadius)
                    {
                        // Calcula la intensidad del color en función de la dureza del pincel
                        float intensity = 1f - (distance / brushRadius);
                        intensity = Mathf.Lerp(intensity, 1f, brushHardness);

                        // Aplica el color con la intensidad calculada
                        Color currentColor = tempTexture.GetPixel(posX, posY);
                        Color blendedColor = Color.Lerp(currentColor, paintColor, intensity);
                        tempTexture.SetPixel(posX, posY, blendedColor);
                    }
                }
            }
        }

        tempTexture.Apply();

        // Copia la textura modificada de vuelta al RenderTexture
        Graphics.Blit(tempTexture, renderTexture);
        RenderTexture.active = null;

        // Libera la memoria usada por la textura temporal
        Destroy(tempTexture);
    }
    private void PaintTextureAtMousePosition()
    {
        // Obtén la posición del mouse en pantalla y conviértela a coordenadas de UV
        Vector2 mousePos = Input.mousePosition;
        float u = (mousePos.x) / Screen.width;
        float v = (mousePos.y ) / Screen.height;

        // Convierte las coordenadas UV a coordenadas de píxeles en el RenderTexture
        int pixelX = (int)(u * renderTexture.width);
        int pixelY = (int)(v * renderTexture.height);

        // Crea una textura temporal para pintar
        Texture2D tempTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Copia los datos del RenderTexture a la textura temporal
        RenderTexture.active = renderTexture;
        tempTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTexture.Apply();

        // Pinta un área circular en torno a la posición del mouse
        for (int y = -Mathf.CeilToInt(brushRadius); y <= Mathf.CeilToInt(brushRadius); y++)
        {
            for (int x = -Mathf.CeilToInt(brushRadius); x <= Mathf.CeilToInt(brushRadius); x++)
            {
                int posX = pixelX + x;
                int posY = pixelY + y;

                // Verifica si el píxel está dentro del radio y dentro de los límites de la textura
                if (posX >= 0 && posX < renderTexture.width && posY >= 0 && posY < renderTexture.height)
                {
                    float distance = Mathf.Sqrt(x * x + y * y);
                    if (distance <= brushRadius)
                    {
                        // Calcula la intensidad del color en función de la dureza del pincel
                        float intensity = 1f - (distance / brushRadius);
                        intensity = Mathf.Lerp(intensity, 1f, brushHardness);

                        // Aplica el color con la intensidad calculada
                        Color currentColor = tempTexture.GetPixel(posX, posY);
                        Color blendedColor = Color.Lerp(currentColor, paintColor, intensity);
                        tempTexture.SetPixel(posX, posY, blendedColor);
                    }
                }
            }
        }

        tempTexture.Apply();

        // Copia la textura modificada de vuelta al RenderTexture
        Graphics.Blit(tempTexture, renderTexture);
        RenderTexture.active = null;

        // Libera la memoria usada por la textura temporal
        Destroy(tempTexture);
    }
    public void SaveRenderTexture(string filePath)
    {
        // Crea una nueva Texture2D con el mismo tamaño que el RenderTexture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Establece el RenderTexture activo
        RenderTexture.active = renderTexture;

        // Copia los datos del RenderTexture a la Texture2D
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Restablece el RenderTexture activo
        RenderTexture.active = null;

        // Convierte la textura a formato PNG
        byte[] bytes = texture.EncodeToPNG();

        // Guarda la imagen en el disco
        File.WriteAllBytes(filePath, bytes);

        // Libera la memoria usada por la Texture2D
        Destroy(texture);

        Debug.Log("Imagen guardada en: " + filePath);
    }
    private void OnDestroy()
    {
        string fullPath = Application.dataPath + "/Resources/Temp/renderTemp.jpg";
        SaveRenderTexture(fullPath);
    }
}
