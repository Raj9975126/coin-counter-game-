#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

[InitializeOnLoad]
public class GameSetup
{
    private const string PREF_KEY = "CoinCatcher_SetupDone_v1";

    static GameSetup()
    {
        EditorApplication.delayCall += CheckAndAutoSetup;
    }

    private static void CheckAndAutoSetup()
    {
        // Only run auto-setup once, or if scene is missing
        if (!EditorPrefs.GetBool(PREF_KEY, false) || !File.Exists("Assets/Scenes/CoinCatcherGame.unity"))
        {
            SetupCompleteGame();
            EditorPrefs.SetBool(PREF_KEY, true);
        }
    }

    [MenuItem("Coin Catcher/🎮 Auto Setup Complete Game Now")]
    public static void SetupCompleteGame()
    {
        Debug.Log("Starting Coin Catcher automatic game setup...");

        // Ensure folders exist
        if (!AssetDatabase.IsValidFolder("Assets/Scenes")) AssetDatabase.CreateFolder("Assets", "Scenes");
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs")) AssetDatabase.CreateFolder("Assets", "Prefabs");

        // 1. Create a fresh scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // 2. Configure Main Camera for 2D
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.orthographic = true;
            mainCam.orthographicSize = 5f;
            mainCam.clearFlags = CameraClearFlags.SolidColor;
            mainCam.backgroundColor = new Color(0.12f, 0.16f, 0.24f); // Rich dark blue
            mainCam.transform.position = new Vector3(0, 0, -10f);
        }

        // Remove Directional Light
        var dirLight = GameObject.Find("Directional Light");
        if (dirLight != null) Object.DestroyImmediate(dirLight);

        // 3. Load Sprites
        Sprite basketSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Basket.png");
        Sprite coinSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Coin.png");

        // 4. Create and Save Coin Prefab
        GameObject coinObj = new GameObject("Coin");
        var coinSr = coinObj.AddComponent<SpriteRenderer>();
        if (coinSprite != null)
        {
            coinSr.sprite = coinSprite;
        }
        else
        {
            coinSr.color = Color.yellow;
        }

        var coinCollider = coinObj.AddComponent<CircleCollider2D>();
        coinCollider.isTrigger = true;
        coinCollider.radius = 0.5f;

        var coinRb = coinObj.AddComponent<Rigidbody2D>();
        coinRb.bodyType = RigidbodyType2D.Kinematic;

        var coinScript = coinObj.AddComponent<Coin>();
        coinScript.fallSpeed = 5f;
        coinScript.scoreValue = 1;
        coinScript.bottomLimit = -5.5f;

        string prefabPath = "Assets/Prefabs/Coin.prefab";
        GameObject coinPrefab = PrefabUtility.SaveAsPrefabAsset(coinObj, prefabPath);
        Object.DestroyImmediate(coinObj);

        // 5. Create Player (Basket)
        GameObject playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.transform.position = new Vector3(0, -4f, 0);
        playerObj.transform.localScale = new Vector3(1.5f, 1.5f, 1f);

        var playerSr = playerObj.AddComponent<SpriteRenderer>();
        if (basketSprite != null)
        {
            playerSr.sprite = basketSprite;
        }
        else
        {
            playerSr.color = new Color(0.7f, 0.45f, 0.2f);
        }

        var playerCollider = playerObj.AddComponent<BoxCollider2D>();
        playerCollider.size = new Vector2(1.8f, 0.6f);

        var playerRb = playerObj.AddComponent<Rigidbody2D>();
        playerRb.bodyType = RigidbodyType2D.Kinematic;

        var playerCtrl = playerObj.AddComponent<PlayerController>();
        playerCtrl.moveSpeed = 10f;
        playerCtrl.xBoundary = 7.5f;

        // 6. Create CoinSpawner
        GameObject spawnerObj = new GameObject("CoinSpawner");
        spawnerObj.transform.position = new Vector3(0, 6f, 0);
        var spawner = spawnerObj.AddComponent<CoinSpawner>();
        spawner.coinPrefab = coinPrefab;
        spawner.spawnInterval = 1.1f;
        spawner.xRange = 7.2f;
        spawner.spawnY = 6.0f;

        // 7. Setup UI Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // EventSystem
        if (Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Score Text
        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(canvasObj.transform, false);
        var scoreTMP = scoreTextObj.AddComponent<TextMeshProUGUI>();
        scoreTMP.text = "Score: 0";
        scoreTMP.fontSize = 42;
        scoreTMP.color = Color.white;
        scoreTMP.fontStyle = FontStyles.Bold;
        RectTransform scoreRect = scoreTextObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.pivot = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(50, -40);
        scoreRect.sizeDelta = new Vector2(300, 60);

        // Lives Text
        GameObject livesTextObj = new GameObject("LivesText");
        livesTextObj.transform.SetParent(canvasObj.transform, false);
        var livesTMP = livesTextObj.AddComponent<TextMeshProUGUI>();
        livesTMP.text = "Lives: 3";
        livesTMP.fontSize = 42;
        livesTMP.color = new Color(1f, 0.35f, 0.35f); // Soft red
        livesTMP.fontStyle = FontStyles.Bold;
        RectTransform livesRect = livesTextObj.GetComponent<RectTransform>();
        livesRect.anchorMin = new Vector2(1, 1);
        livesRect.anchorMax = new Vector2(1, 1);
        livesRect.pivot = new Vector2(1, 1);
        livesRect.anchoredPosition = new Vector2(-50, -40);
        livesRect.sizeDelta = new Vector2(300, 60);

        // GameOver Panel
        GameObject panelObj = new GameObject("GameOverPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);
        var panelImg = panelObj.AddComponent<Image>();
        panelImg.color = new Color(0, 0, 0, 0.85f);
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        // GameOver Title
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panelObj.transform, false);
        var titleTMP = titleObj.AddComponent<TextMeshProUGUI>();
        titleTMP.text = "GAME OVER";
        titleTMP.fontSize = 72;
        titleTMP.fontStyle = FontStyles.Bold;
        titleTMP.color = new Color(1f, 0.25f, 0.25f);
        titleTMP.alignment = TextAlignmentOptions.Center;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 120);
        titleRect.sizeDelta = new Vector2(600, 100);

        // Final Score Text
        GameObject finalScoreObj = new GameObject("FinalScoreText");
        finalScoreObj.transform.SetParent(panelObj.transform, false);
        var finalScoreTMP = finalScoreObj.AddComponent<TextMeshProUGUI>();
        finalScoreTMP.text = "Final Score: 0";
        finalScoreTMP.fontSize = 44;
        finalScoreTMP.color = Color.white;
        finalScoreTMP.alignment = TextAlignmentOptions.Center;
        RectTransform finalScoreRect = finalScoreObj.GetComponent<RectTransform>();
        finalScoreRect.anchoredPosition = new Vector2(0, 20);
        finalScoreRect.sizeDelta = new Vector2(500, 80);

        // Restart Button
        GameObject btnObj = new GameObject("RestartButton");
        btnObj.transform.SetParent(panelObj.transform, false);
        var btnImg = btnObj.AddComponent<Image>();
        btnImg.color = new Color(0.2f, 0.7f, 0.3f); // Fresh green
        var btn = btnObj.AddComponent<Button>();
        RectTransform btnRect = btnObj.GetComponent<RectTransform>();
        btnRect.anchoredPosition = new Vector2(0, -90);
        btnRect.sizeDelta = new Vector2(240, 70);

        GameObject btnTextObj = new GameObject("Text");
        btnTextObj.transform.SetParent(btnObj.transform, false);
        var btnTMP = btnTextObj.AddComponent<TextMeshProUGUI>();
        btnTMP.text = "RESTART";
        btnTMP.fontSize = 32;
        btnTMP.fontStyle = FontStyles.Bold;
        btnTMP.color = Color.white;
        btnTMP.alignment = TextAlignmentOptions.Center;
        RectTransform btnTextRect = btnTextObj.GetComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.sizeDelta = Vector2.zero;

        panelObj.SetActive(false);

        // 8. Setup GameManager
        GameObject gmObj = new GameObject("GameManager");
        var audioSource = gmObj.AddComponent<AudioSource>();
        var gm = gmObj.AddComponent<GameManager>();
        gm.scoreText = scoreTMP;
        gm.livesText = livesTMP;
        gm.gameOverPanel = panelObj;
        gm.finalScoreText = finalScoreTMP;
        gm.audioSource = audioSource;

        // Load Audio Clips
        gm.coinCatchSound = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/coin_catch.wav");
        gm.coinMissSound = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/coin_miss.wav");
        gm.gameOverSound = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/game_over.wav");

        // Connect button to GameManager.RestartGame
        UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, gm.RestartGame);

        // 9. Save Scene
        string scenePath = "Assets/Scenes/CoinCatcherGame.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("Scene saved successfully at: " + scenePath);

        // 10. Update Build Settings
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(scenePath, true)
        };
        EditorBuildSettings.scenes = scenes;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Coin Catcher Setup", 
            "All game objects, prefabs, spawner, UI, and scripts have been created and linked automatically!\n\nJust click the Play button (▶) at the top to play!", 
            "Awesome!");
    }
}
#endif
