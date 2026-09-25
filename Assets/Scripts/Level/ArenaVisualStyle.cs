using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaVisualStyle : MonoBehaviour
{
    [Header("Auto Setup")]
    [SerializeField] private string generatedArenaName = "GeneratedArena";
    [SerializeField] private bool applyOnStart = true;

    [Header("Ground")]
    [SerializeField] private Color groundColor = new Color(0.48f, 0.32f, 0.16f, 1f);
    [SerializeField] private float groundSmoothness = 0.05f;

    [Header("Wood")]
    [SerializeField] private Color woodColor = new Color(0.25f, 0.12f, 0.045f, 1f);
    [SerializeField] private Color woodLightColor = new Color(0.38f, 0.20f, 0.08f, 1f);
    [SerializeField] private Color woodDarkColor = new Color(0.16f, 0.07f, 0.025f, 1f);
    [SerializeField] private float woodSmoothness = 0.15f;

    [Header("Wall Visuals")]
    [SerializeField] private float plankWidth = 0.75f;
    [SerializeField] private float plankGap = 0.035f;
    [SerializeField] private float beamThickness = 0.16f;
    [SerializeField] private float beamInset = 0.08f;

    [Header("Cover Visuals")]
    [SerializeField] private bool createCoverBoards = true;
    [SerializeField] private float coverBoardHeight = 0.16f;
    [SerializeField] private float coverBoardDepth = 0.10f;

    [Header("Lighting")]
    [SerializeField] private bool configureDirectionalLight = true;
    [SerializeField] private Color sunlightColor =
        new Color(1f, 0.78f, 0.58f, 1f);
    [SerializeField] private float sunlightIntensity = 1.1f;
    [SerializeField] private Vector3 sunlightRotation =
        new Vector3(38f, -32f, 0f);

    private Material groundMaterial;
    private Material woodMaterial;
    private Material woodLightMaterial;
    private Material woodDarkMaterial;

    private Transform generatedArena;
    private readonly HashSet<int> processedObjects =
        new HashSet<int>();

    private IEnumerator Start()
    {
        if (!applyOnStart)
            yield break;

        CreateMaterials();

        if (configureDirectionalLight)
        {
            ConfigureLighting();
        }

        // ArenaGenerator creates GeneratedArena in its Start().
        // Wait until that object actually exists.
        while (generatedArena == null)
        {
            GameObject arena =
                GameObject.Find(generatedArenaName);

            if (arena != null)
            {
                generatedArena = arena.transform;
                break;
            }

            yield return null;
        }

        // Give ArenaGenerator one frame to finish creating
        // rooms, walls, corridors and covers.
        yield return null;

        ApplyVisuals();
    }

    private void CreateMaterials()
    {
        groundMaterial =
            CreateMaterial(
                "M_Visual_Ground",
                groundColor,
                0f,
                groundSmoothness
            );

        woodMaterial =
            CreateMaterial(
                "M_Visual_Wood",
                woodColor,
                0f,
                woodSmoothness
            );

        woodLightMaterial =
            CreateMaterial(
                "M_Visual_Wood_Light",
                woodLightColor,
                0f,
                woodSmoothness
            );

        woodDarkMaterial =
            CreateMaterial(
                "M_Visual_Wood_Dark",
                woodDarkColor,
                0f,
                woodSmoothness
            );
    }

    private Material CreateMaterial(
        string materialName,
        Color color,
        float metallic,
        float smoothness
    )
    {
        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit"
            );

        if (shader == null)
        {
            shader =
                Shader.Find("Standard");
        }

        if (shader == null)
            return null;

        Material material =
            new Material(shader);

        material.name =
            materialName;

        material.color =
            color;

        if (material.HasProperty("_Metallic"))
        {
            material.SetFloat(
                "_Metallic",
                metallic
            );
        }

        if (material.HasProperty("_Smoothness"))
        {
            material.SetFloat(
                "_Smoothness",
                smoothness
            );
        }

        return material;
    }

    private void ApplyVisuals()
    {
        if (generatedArena == null)
            return;

        Transform[] objects =
            generatedArena.GetComponentsInChildren<
                Transform
            >(true);

        int floors = 0;
        int walls = 0;
        int covers = 0;

        foreach (Transform current in objects)
        {
            if (current == null)
                continue;

            if (current == generatedArena)
                continue;

            if (current.name == "Floor" ||
                current.name == "CorridorFloor")
            {
                if (processedObjects.Add(
                    current.gameObject.GetInstanceID()
                ))
                {
                    CreateGroundVisual(current);
                    floors++;
                }

                continue;
            }

            if (IsWallName(current.name))
            {
                if (processedObjects.Add(
                    current.gameObject.GetInstanceID()
                ))
                {
                    CreateWallVisual(current);
                    walls++;
                }

                continue;
            }

            if (current.name.StartsWith("Cover_"))
            {
                if (processedObjects.Add(
                    current.gameObject.GetInstanceID()
                ))
                {
                    CreateCoverVisual(current);
                    covers++;
                }
            }
        }

        Debug.Log(
            $"ArenaVisualStyle: Applied visuals. " +
            $"Floors={floors}, Walls={walls}, Covers={covers}"
        );
    }

    private bool IsWallName(string objectName)
    {
        if (string.IsNullOrEmpty(objectName))
            return false;

        if (objectName.StartsWith("Wall_"))
            return true;

        if (objectName.StartsWith("CorridorWall_"))
            return true;

        return false;
    }

    private void CreateGroundVisual(
        Transform source
    )
    {
        Renderer sourceRenderer =
            source.GetComponent<Renderer>();

        if (sourceRenderer == null)
            return;

        Vector3 sourceScale =
            source.lossyScale;

        Transform parent =
            source.parent;

        GameObject visual =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        visual.name =
            source.name + "_Visual";

        if (parent != null)
        {
            visual.transform.SetParent(
                parent,
                true
            );
        }

        visual.transform.position =
            source.position +
            Vector3.up * 0.006f;

        visual.transform.rotation =
            source.rotation;

        visual.transform.localScale =
            new Vector3(
                sourceScale.x,
                0.012f,
                sourceScale.z
            );

        RemoveCollider(visual);

        Renderer renderer =
            visual.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial =
                groundMaterial;
        }

        sourceRenderer.enabled =
            false;
    }

    private void CreateWallVisual(
        Transform source
    )
    {
        Renderer sourceRenderer =
            source.GetComponent<Renderer>();

        if (sourceRenderer == null)
            return;

        Vector3 size =
            GetWorldSize(source);

        Transform parent =
            source.parent;

        GameObject visualRoot =
            new GameObject(
                source.name + "_Visual"
            );

        if (parent != null)
        {
            visualRoot.transform.SetParent(
                parent,
                true
            );
        }

        visualRoot.transform.position =
            source.position;

        visualRoot.transform.rotation =
            source.rotation;

        visualRoot.transform.localScale =
            Vector3.one;

        float wallWidth =
            size.x;

        float wallHeight =
            size.y;

        float wallDepth =
            size.z;

        // Because CreateWall rotates the cube so that
        // its local Z axis is the wall length.
        // We build the visual in the wall's local space.
        CreateWallPlanks(
            visualRoot.transform,
            wallWidth,
            wallHeight,
            wallDepth
        );

        CreateWallBeams(
            visualRoot.transform,
            wallWidth,
            wallHeight,
            wallDepth
        );

        sourceRenderer.enabled =
            false;
    }

    private Vector3 GetWorldSize(
        Transform source
    )
    {
        Renderer renderer =
            source.GetComponent<Renderer>();

        if (renderer != null)
        {
            return renderer.bounds.size;
        }

        return Vector3.one;
    }

    private void CreateWallPlanks(
        Transform root,
        float width,
        float height,
        float depth
    )
    {
        float usableWidth =
            Mathf.Max(
                0.5f,
                width - 0.08f
            );

        int plankCount =
            Mathf.Max(
                1,
                Mathf.FloorToInt(
                    usableWidth /
                    Mathf.Max(
                        0.1f,
                        plankWidth +
                        plankGap
                    )
                )
            );

        float actualPlankWidth =
            usableWidth /
            plankCount;

        for (
            int i = 0;
            i < plankCount;
            i++
        )
        {
            float x =
                -usableWidth * 0.5f +
                actualPlankWidth * 0.5f +
                i * actualPlankWidth;

            GameObject plank =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            plank.name =
                "WoodPlank_" +
                i;

            plank.transform.SetParent(
                root,
                false
            );

            plank.transform.localPosition =
                new Vector3(
                    x,
                    0f,
                    0f
                );

            plank.transform.localRotation =
                Quaternion.identity;

            plank.transform.localScale =
                new Vector3(
                    Mathf.Max(
                        0.05f,
                        actualPlankWidth -
                        plankGap
                    ),
                    height,
                    depth + 0.025f
                );

            RemoveCollider(plank);

            Renderer renderer =
                plank.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    GetPlankMaterial(i);
            }
        }
    }

    private Material GetPlankMaterial(
        int index
    )
    {
        int variant =
            index % 5;

        if (variant == 0)
            return woodDarkMaterial;

        if (variant == 3)
            return woodLightMaterial;

        return woodMaterial;
    }

    private void CreateWallBeams(
        Transform root,
        float width,
        float height,
        float depth
    )
    {
        float beamDepth =
            depth +
            beamInset;

        // Bottom beam
        CreateBeam(
            root,
            "WoodBeam_Bottom",
            new Vector3(
                0f,
                -height * 0.5f +
                beamThickness * 0.5f,
                0f
            ),
            new Vector3(
                width + 0.08f,
                beamThickness,
                beamDepth
            )
        );

        // Top beam
        CreateBeam(
            root,
            "WoodBeam_Top",
            new Vector3(
                0f,
                height * 0.5f -
                beamThickness * 0.5f,
                0f
            ),
            new Vector3(
                width + 0.08f,
                beamThickness,
                beamDepth
            )
        );

        // Vertical side beams
        CreateBeam(
            root,
            "WoodBeam_Left",
            new Vector3(
                -width * 0.5f +
                beamThickness * 0.5f,
                0f,
                -beamInset * 0.5f
            ),
            new Vector3(
                beamThickness,
                height,
                beamDepth
            )
        );

        CreateBeam(
            root,
            "WoodBeam_Right",
            new Vector3(
                width * 0.5f -
                beamThickness * 0.5f,
                0f,
                -beamInset * 0.5f
            ),
            new Vector3(
                beamThickness,
                height,
                beamDepth
            )
        );
    }

    private void CreateBeam(
        Transform root,
        string beamName,
        Vector3 localPosition,
        Vector3 localScale
    )
    {
        GameObject beam =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        beam.name =
            beamName;

        beam.transform.SetParent(
            root,
            false
        );

        beam.transform.localPosition =
            localPosition;

        beam.transform.localRotation =
            Quaternion.identity;

        beam.transform.localScale =
            localScale;

        RemoveCollider(beam);

        Renderer renderer =
            beam.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial =
                woodDarkMaterial;
        }
    }

    private void CreateCoverVisual(
        Transform source
    )
    {
        Renderer sourceRenderer =
            source.GetComponent<Renderer>();

        if (sourceRenderer == null)
            return;

        Vector3 size =
            sourceRenderer.bounds.size;

        Transform parent =
            source.parent;

        GameObject visualRoot =
            new GameObject(
                source.name + "_Visual"
            );

        if (parent != null)
        {
            visualRoot.transform.SetParent(
                parent,
                true
            );
        }

        visualRoot.transform.position =
            source.position;

        visualRoot.transform.rotation =
            source.rotation;

        visualRoot.transform.localScale =
            Vector3.one;

        CreateCoverMainBody(
            visualRoot.transform,
            size
        );

        if (createCoverBoards)
        {
            CreateCoverTopBoards(
                visualRoot.transform,
                size
            );
        }

        sourceRenderer.enabled =
            false;
    }

    private void CreateCoverMainBody(
        Transform root,
        Vector3 size
    )
    {
        GameObject body =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        body.name =
            "CoverWoodBody";

        body.transform.SetParent(
            root,
            false
        );

        body.transform.localPosition =
            Vector3.zero;

        body.transform.localRotation =
            Quaternion.identity;

        body.transform.localScale =
            new Vector3(
                size.x,
                size.y,
                size.z
            );

        RemoveCollider(body);

        Renderer renderer =
            body.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial =
                woodMaterial;
        }
    }

    private void CreateCoverTopBoards(
        Transform root,
        Vector3 size
    )
    {
        float boardThickness =
            Mathf.Max(
                0.05f,
                coverBoardHeight
            );

        float boardDepth =
            Mathf.Max(
                0.04f,
                coverBoardDepth
            );

        float boardWidth =
            Mathf.Max(
                0.25f,
                size.x * 0.9f
            );

        Vector3 position =
            new Vector3(
                0f,
                size.y * 0.5f +
                boardThickness * 0.5f,
                0f
            );

        GameObject board =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        board.name =
            "CoverTopBoard";

        board.transform.SetParent(
            root,
            false
        );

        board.transform.localPosition =
            position;

        board.transform.localRotation =
            Quaternion.identity;

        board.transform.localScale =
            new Vector3(
                boardWidth,
                boardThickness,
                size.z + boardDepth
            );

        RemoveCollider(board);

        Renderer renderer =
            board.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial =
                woodLightMaterial;
        }
    }

    private void RemoveCollider(
        GameObject obj
    )
    {
        Collider collider =
            obj.GetComponent<Collider>();

        if (collider != null)
        {
            Destroy(collider);
        }
    }

    private void ConfigureLighting()
    {
        Light[] lights =
            FindObjectsByType<Light>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        Light directional =
            null;

        foreach (Light light in lights)
        {
            if (light == null)
                continue;

            if (
                light.type ==
                LightType.Directional
            )
            {
                directional = light;
                break;
            }
        }

        if (directional == null)
        {
            GameObject lightObject =
                new GameObject(
                    "Directional Light"
                );

            directional =
                lightObject.AddComponent<Light>();

            directional.type =
                LightType.Directional;
        }

        directional.transform.rotation =
            Quaternion.Euler(
                sunlightRotation
            );

        directional.color =
            sunlightColor;

        directional.intensity =
            sunlightIntensity;

        directional.shadows =
            LightShadows.Soft;

        directional.shadowStrength =
            0.8f;

        directional.shadowBias =
            0.05f;

        directional.shadowNormalBias =
            0.4f;
    }

    private void OnDestroy()
    {
        DestroyRuntimeMaterial(
            groundMaterial
        );

        DestroyRuntimeMaterial(
            woodMaterial
        );

        DestroyRuntimeMaterial(
            woodLightMaterial
        );

        DestroyRuntimeMaterial(
            woodDarkMaterial
        );
    }

    private void DestroyRuntimeMaterial(
        Material material
    )
    {
        if (material != null)
        {
            Destroy(material);
        }
    }
}