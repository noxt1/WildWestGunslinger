using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyDirectionIndicator : MonoBehaviour
{
    private static EnemyDirectionIndicator instance;

    [Header("Indicator")]
    [SerializeField] private float ringRadius = 190f;
    [SerializeField] private float ringThickness = 5f;
    [SerializeField] private float arrowSize = 42f;
    [SerializeField] private int maxArrows = 6;

    [Header("Refresh")]
    [SerializeField] private float refreshInterval = 0.12f;

    private Canvas canvas;
    private RectTransform root;

    private Image ringImage;

    private readonly List<Image> arrows =
        new List<Image>();

    private Transform player;
    private Camera mainCamera;

    private float refreshTimer;

    private Sprite ringSprite;
    private Sprite arrowSprite;

    private readonly List<EnemyController> enemies =
        new List<EnemyController>();

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.AfterSceneLoad
    )]
    private static void Bootstrap()
    {
        if (instance != null)
        {
            return;
        }

        GameObject obj =
            new GameObject(
                "EnemyDirectionIndicator"
            );

        instance =
            obj.AddComponent<
                EnemyDirectionIndicator
            >();

        DontDestroyOnLoad(
            obj
        );
    }

    private void Awake()
    {
        if (instance != null &&
            instance != this)
        {
            Destroy(
                gameObject
            );

            return;
        }

        instance =
            this;

        DontDestroyOnLoad(
            gameObject
        );

        CreateUI();

        SceneManager.sceneLoaded +=
            OnSceneLoaded;

        RefreshScene();
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode)
    {
        refreshTimer = 0f;

        RefreshScene();
    }

    private void Update()
    {
        if (canvas == null)
        {
            return;
        }

        if (player == null)
        {
            RefreshScene();
        }

        if (player == null)
        {
            HideIndicator();
            return;
        }

        if (mainCamera == null)
        {
            mainCamera =
                Camera.main;
        }

        if (mainCamera == null)
        {
            HideIndicator();
            return;
        }

        refreshTimer +=
            Time.unscaledDeltaTime;

        if (
            refreshTimer >=
            refreshInterval
        )
        {
            refreshTimer = 0f;

            RefreshEnemies();
        }

        UpdateArrows();
    }

    private void RefreshScene()
    {
        PlayerController playerController =
            FindFirstObjectByType<
                PlayerController
            >();

        if (playerController == null)
        {
            player = null;

            HideIndicator();

            return;
        }

        player =
            playerController.transform;

        mainCamera =
            Camera.main;

        ShowIndicator();

        RefreshEnemies();
    }

    private void CreateUI()
    {
        GameObject canvasObject =
            new GameObject(
                "EnemyDirectionIndicatorCanvas"
            );

        canvasObject.transform.SetParent(
            transform,
            false
        );

        canvas =
            canvasObject.AddComponent<
                Canvas
            >();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder =
            5000;

        CanvasScaler scaler =
            canvasObject.AddComponent<
                CanvasScaler
            >();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode
                .ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(
                1920f,
                1080f
            );

        canvasObject.AddComponent<
            GraphicRaycaster
        >();

        GameObject rootObject =
            new GameObject(
                "IndicatorRoot"
            );

        rootObject.transform.SetParent(
            canvas.transform,
            false
        );

        root =
            rootObject.AddComponent<
                RectTransform
            >();

        root.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        root.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        root.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        root.anchoredPosition =
            Vector2.zero;

        root.sizeDelta =
            new Vector2(
                ringRadius * 2f +
                100f,
                ringRadius * 2f +
                100f
            );

        CreateRing();
        CreateArrows();
    }

    private void CreateRing()
    {
        GameObject ringObject =
            new GameObject(
                "EnemyDirectionRing"
            );

        ringObject.transform.SetParent(
            root,
            false
        );

        RectTransform ringRect =
            ringObject.AddComponent<
                RectTransform
            >();

        ringRect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        ringRect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        ringRect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        ringRect.anchoredPosition =
            Vector2.zero;

        ringRect.sizeDelta =
            new Vector2(
                ringRadius * 2f,
                ringRadius * 2f
            );

        ringImage =
            ringObject.AddComponent<
                Image
            >();

        ringSprite =
            CreateRingSprite(
                256,
                0.90f,
                0.78f
            );

        ringImage.sprite =
            ringSprite;

        ringImage.raycastTarget =
            false;

        ringImage.color =
            new Color(
                1f,
                1f,
                1f,
                0.45f
            );
    }

    private void CreateArrows()
    {
        arrowSprite =
            CreateArrowSprite(
                128
            );

        for (
            int i = 0;
            i < maxArrows;
            i++
        )
        {
            GameObject arrowObject =
                new GameObject(
                    "EnemyDirectionArrow_" +
                    i
                );

            arrowObject.transform.SetParent(
                root,
                false
            );

            RectTransform arrowRect =
                arrowObject.AddComponent<
                    RectTransform
                >();

            arrowRect.anchorMin =
                new Vector2(
                    0.5f,
                    0.5f
                );

            arrowRect.anchorMax =
                new Vector2(
                    0.5f,
                    0.5f
                );

            arrowRect.pivot =
                new Vector2(
                    0.5f,
                    0.5f
                );

            arrowRect.sizeDelta =
                new Vector2(
                    arrowSize,
                    arrowSize
                );

            Image image =
                arrowObject.AddComponent<
                    Image
                >();

            image.sprite =
                arrowSprite;

            image.raycastTarget =
                false;

            image.color =
                new Color(
                    1f,
                    0.18f,
                    0.05f,
                    0.95f
                );

            arrows.Add(
                image
            );

            arrowObject.SetActive(
                false
            );
        }
    }

    private void RefreshEnemies()
    {
        enemies.Clear();

        EnemyController[] found =
            FindObjectsByType<
                EnemyController
            >(
                FindObjectsSortMode.None
            );

        if (found == null)
        {
            return;
        }

        foreach (
            EnemyController enemy
            in found
        )
        {
            if (enemy == null)
            {
                continue;
            }

            enemies.Add(
                enemy
            );
        }
    }

    private void UpdateArrows()
    {
        if (
            player == null ||
            mainCamera == null
        )
        {
            HideArrows();
            return;
        }

        List<EnemyDistance> visibleEnemies =
            new List<EnemyDistance>();

        foreach (
            EnemyController enemy
            in enemies
        )
        {
            if (enemy == null)
            {
                continue;
            }

            EnemyHealth health =
                enemy.GetComponentInChildren<
                    EnemyHealth
                >();

            if (
                health != null &&
                health.GetType() != null
            )
            {
                /*
                 * Сам EnemyHealth используется только
                 * для гарантии, что это полноценный враг.
                 */
            }

            Vector3 direction =
                enemy.transform.position -
                player.position;

            float distance =
                direction.magnitude;

            if (distance <= 0.01f)
            {
                continue;
            }

            visibleEnemies.Add(
                new EnemyDistance
                {
                    enemy = enemy,
                    distance = distance
                }
            );
        }

        visibleEnemies.Sort(
            (
                a,
                b
            ) =>
                a.distance.CompareTo(
                    b.distance
                )
        );

        int count =
            Mathf.Min(
                maxArrows,
                visibleEnemies.Count
            );

        for (
            int i = 0;
            i < arrows.Count;
            i++
        )
        {
            if (i >= count)
            {
                arrows[i]
                    .gameObject
                    .SetActive(
                        false
                    );

                continue;
            }

            EnemyController enemy =
                visibleEnemies[i].enemy;

            Vector3 worldDirection =
                enemy.transform.position -
                player.position;

            worldDirection.y =
                0f;

            if (
                worldDirection.sqrMagnitude <
                0.0001f
            )
            {
                arrows[i]
                    .gameObject
                    .SetActive(
                        false
                    );

                continue;
            }

            worldDirection.Normalize();

            Vector3 cameraRight =
                mainCamera.transform.right;

            Vector3 cameraUp =
                mainCamera.transform.up;

            float x =
                Vector3.Dot(
                    worldDirection,
                    cameraRight
                );

            float y =
                Vector3.Dot(
                    worldDirection,
                    cameraUp
                );

            Vector2 screenDirection =
                new Vector2(
                    x,
                    y
                );

            screenDirection.y =
                Mathf.Clamp(
                    screenDirection.y,
                    -1f,
                    1f
                );

            if (
                screenDirection.sqrMagnitude <
                0.0001f
            )
            {
                screenDirection =
                    Vector2.up;
            }

            screenDirection.Normalize();

            RectTransform arrowRect =
                arrows[i]
                    .rectTransform;

            arrowRect.anchoredPosition =
                screenDirection *
                ringRadius;

            float angle =
                Mathf.Atan2(
                    screenDirection.y,
                    screenDirection.x
                ) *
                Mathf.Rad2Deg;

            /*
             * Наш треугольник смотрит вправо,
             * поэтому поворачиваем его по экранному
             * направлению.
             */
            arrowRect.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    angle
                );

            arrows[i]
                .gameObject
                .SetActive(
                    true
                );
        }
    }

    private void HideIndicator()
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(
                false
            );
        }
    }

    private void ShowIndicator()
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(
                true
            );
        }
    }

    private void HideArrows()
    {
        foreach (
            Image arrow
            in arrows
        )
        {
            if (arrow != null)
            {
                arrow.gameObject.SetActive(
                    false
                );
            }
        }
    }

    private Sprite CreateRingSprite(
        int size,
        float outerRadius,
        float innerRadius)
    {
        Texture2D texture =
            new Texture2D(
                size,
                size,
                TextureFormat.RGBA32,
                false
            );

        texture.filterMode =
            FilterMode.Bilinear;

        Color[] pixels =
            new Color[
                size * size
            ];

        Vector2 center =
            new Vector2(
                size * 0.5f,
                size * 0.5f
            );

        float maxRadius =
            size * 0.5f;

        for (
            int y = 0;
            y < size;
            y++
        )
        {
            for (
                int x = 0;
                x < size;
                x++
            )
            {
                Vector2 p =
                    new Vector2(
                        x,
                        y
                    );

                float normalizedDistance =
                    Vector2.Distance(
                        p,
                        center
                    ) /
                    maxRadius;

                if (
                    normalizedDistance <=
                    outerRadius &&
                    normalizedDistance >=
                    innerRadius
                )
                {
                    pixels[
                        y * size + x
                    ] =
                        Color.white;
                }
                else
                {
                    pixels[
                        y * size + x
                    ] =
                        Color.clear;
                }
            }
        }

        texture.SetPixels(
            pixels
        );

        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(
                0f,
                0f,
                size,
                size
            ),
            new Vector2(
                0.5f,
                0.5f
            ),
            100f
        );
    }

    private Sprite CreateArrowSprite(
        int size)
    {
        Texture2D texture =
            new Texture2D(
                size,
                size,
                TextureFormat.RGBA32,
                false
            );

        texture.filterMode =
            FilterMode.Bilinear;

        Color[] pixels =
            new Color[
                size * size
            ];

        for (
            int y = 0;
            y < size;
            y++
        )
        {
            for (
                int x = 0;
                x < size;
                x++
            )
            {
                pixels[
                    y * size + x
                ] =
                    Color.clear;
            }
        }

        /*
         * Треугольная стрелка,
         * направленная вправо.
         */
        for (
            int y = 0;
            y < size;
            y++
        )
        {
            float normalizedY =
                Mathf.Abs(
                    y -
                    size * 0.5f
                ) /
                (size * 0.5f);

            float width =
                (1f -
                 normalizedY) *
                size *
                0.42f;

            int startX =
                Mathf.RoundToInt(
                    size * 0.30f -
                    width * 0.5f
                );

            int endX =
                Mathf.RoundToInt(
                    size * 0.30f +
                    width * 0.5f
                );

            int tipX =
                Mathf.RoundToInt(
                    size * 0.82f
                );

            for (
                int x = startX;
                x <= tipX;
                x++
            )
            {
                if (
                    x < 0 ||
                    x >= size
                )
                {
                    continue;
                }

                float t =
                    Mathf.InverseLerp(
                        startX,
                        tipX,
                        x
                    );

                float currentWidth =
                    width *
                    (1f - t);

                if (
                    Mathf.Abs(
                        y -
                        size * 0.5f
                    ) <=
                    currentWidth
                )
                {
                    pixels[
                        y * size + x
                    ] =
                        Color.white;
                }
            }
        }

        texture.SetPixels(
            pixels
        );

        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(
                0f,
                0f,
                size,
                size
            ),
            new Vector2(
                0.5f,
                0.5f
            ),
            100f
        );
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -=
            OnSceneLoaded;

        if (ringSprite != null)
        {
            Destroy(
                ringSprite.texture
            );

            Destroy(
                ringSprite
            );
        }

        if (arrowSprite != null)
        {
            Destroy(
                arrowSprite.texture
            );

            Destroy(
                arrowSprite
            );
        }
    }

    private struct EnemyDistance
    {
        public EnemyController enemy;
        public float distance;
    }
}