using System.Collections.Generic;
using UnityEngine;

public class WildWestEnvironmentGenerator : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private bool autoGenerate = true;

    [Header("Seed")]
    [SerializeField] private int seed = 12345;
    [SerializeField] private bool randomizeSeed = false;

    [Header("Bounds")]
    [SerializeField] private float additionalBoundsMargin = 2f;
    [SerializeField] private float exitKeepClearRadius = 3f;

    [Header("Preview")]
    [SerializeField] private bool previewOnly = true;

    [Header("Ground")]
    [SerializeField] private bool groundEnabled = true;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private float groundExtraMargin = 30f;
    [SerializeField] private float groundHeight = -0.05f;

    [Header("Cliffs")]
    [SerializeField] private bool cliffsEnabled = true;
    [SerializeField] private GameObject[] cliffPrefabs;
    [SerializeField] private float cliffMinDistance = 8f;
    [SerializeField] private float cliffMaxDistance = 70f;
    [SerializeField] private float cliffDensity = 0.7f;
    [SerializeField] private int cliffMaxCount = 8;
    [SerializeField] private float cliffMinSpacing = 12f;
    [SerializeField] private float cliffScaleMin = 0.8f;
    [SerializeField] private float cliffScaleMax = 1.6f;
    [SerializeField] private float cliffClearance = 6f;
    [SerializeField] private bool cliffRandomRotation = true;

    [Header("Rocks")]
    [SerializeField] private bool rocksEnabled = true;
    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField] private float rockMinDistance = 4f;
    [SerializeField] private float rockMaxDistance = 75f;
    [SerializeField] private float rockDensity = 0.8f;
    [SerializeField] private int rockMaxCount = 40;
    [SerializeField] private float rockMinSpacing = 5f;
    [SerializeField] private float rockScaleMin = 0.5f;
    [SerializeField] private float rockScaleMax = 1.2f;
    [SerializeField] private float rockClearance = 3f;
    [SerializeField] private bool rockRandomRotation = true;

    [Header("Trees")]
    [SerializeField] private bool treesEnabled = true;
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private float treeMinDistance = 12f;
    [SerializeField] private float treeMaxDistance = 30f;
    [SerializeField] private float treeDensity = 0.65f;
    [SerializeField] private int treeMaxCount = 24;
    [SerializeField] private float treeMinSpacing = 10f;
    [SerializeField] private float treeScaleMin = 0.8f;
    [SerializeField] private float treeScaleMax = 1.4f;
    [SerializeField] private float treeClearance = 8f;
    [SerializeField] private bool treeRandomRotation = true;

    [Header("Bushes")]
    [SerializeField] private bool bushesEnabled = true;
    [SerializeField] private GameObject[] bushPrefabs;
    [SerializeField] private float bushMinDistance = 5f;
    [SerializeField] private float bushMaxDistance = 30f;
    [SerializeField] private float bushDensity = 0.8f;
    [SerializeField] private int bushMaxCount = 60;
    [SerializeField] private float bushMinSpacing = 4f;
    [SerializeField] private float bushScaleMin = 0.6f;
    [SerializeField] private float bushScaleMax = 1.3f;
    [SerializeField] private float bushClearance = 4f;
    [SerializeField] private bool bushRandomRotation = true;

    [Header("Grass")]
    [SerializeField] private bool grassEnabled = true;
    [SerializeField] private GameObject[] grassPrefabs;
    [SerializeField] private float grassMinDistance = 3f;
    [SerializeField] private float grassMaxDistance = 30f;
    [SerializeField] private float grassDensity = 0.85f;
    [SerializeField] private int grassMaxCount = 300;
    [SerializeField] private float grassMinSpacing = 1.2f;
    [SerializeField] private float grassScaleMin = 0.7f;
    [SerializeField] private float grassScaleMax = 1.5f;
    [SerializeField] private float grassClearance = 2f;
    [SerializeField] private bool grassRandomRotation = true;

    [Header("Fences (destructible cover props, вне комнат — не ломают A*)")]
    [SerializeField] private bool fencesEnabled = true;
    [SerializeField] private GameObject[] fencePrefabs;
    [SerializeField] private float fenceMinDistance = 4f;
    [SerializeField] private float fenceMaxDistance = 30f;
    [SerializeField] private float fenceDensity = 0.7f;
    [SerializeField] private int fenceMaxCount = 12;
    [SerializeField] private float fenceMinSpacing = 6f;
    [SerializeField] private float fenceScaleMin = 0.9f;
    [SerializeField] private float fenceScaleMax = 1.1f;
    [SerializeField] private float fenceClearance = 3f;
    [SerializeField] private bool fenceRandomRotation = true;

    [Header("Props (Barrel/Crate/Cover — вне комнат, не ломают A*)")]
    [SerializeField] private bool propsEnabled = true;
    [SerializeField] private GameObject[] propPrefabs;
    [SerializeField] private float propMinDistance = 4f;
    [SerializeField] private float propMaxDistance = 30f;
    [SerializeField] private float propDensity = 0.7f;
    [SerializeField] private int propMaxCount = 10;
    [SerializeField] private float propMinSpacing = 5f;
    [SerializeField] private float propScaleMin = 0.9f;
    [SerializeField] private float propScaleMax = 1.1f;
    [SerializeField] private float propClearance = 3f;
    [SerializeField] private bool propRandomRotation = true;

    [Header("Lanterns (декор, без CoverPoint и без realtime Light)")]
    [SerializeField] private bool lanternsEnabled = true;
    [SerializeField] private GameObject[] lanternPrefabs;
    [SerializeField] private float lanternMinDistance = 6f;
    [SerializeField] private float lanternMaxDistance = 32f;
    [SerializeField] private float lanternDensity = 0.6f;
    [SerializeField] private int lanternMaxCount = 6;
    [SerializeField] private float lanternMinSpacing = 10f;
    [SerializeField] private float lanternScaleMin = 0.95f;
    [SerializeField] private float lanternScaleMax = 1.05f;
    [SerializeField] private float lanternClearance = 4f;
    [SerializeField] private bool lanternRandomRotation = true;

    [Header("Wagons (крупный cover, вне комнат)")]
    [SerializeField] private bool wagonsEnabled = true;
    [SerializeField] private GameObject[] wagonPrefabs;
    [SerializeField] private float wagonMinDistance = 8f;
    [SerializeField] private float wagonMaxDistance = 35f;
    [SerializeField] private float wagonDensity = 0.6f;
    [SerializeField] private int wagonMaxCount = 4;
    [SerializeField] private float wagonMinSpacing = 14f;
    [SerializeField] private float wagonScaleMin = 0.95f;
    [SerializeField] private float wagonScaleMax = 1.05f;
    [SerializeField] private float wagonClearance = 6f;
    [SerializeField] private bool wagonRandomRotation = true;

    [Header("Background")]
    [SerializeField] private bool backgroundEnabled = true;
    [SerializeField] private GameObject[] backgroundPrefabs;
    [SerializeField] private float backgroundMinDistance = 60f;
    [SerializeField] private float backgroundMaxDistance = 80f;
    [SerializeField] private float backgroundDensity = 0.7f;
    [SerializeField] private int backgroundMaxCount = 6;
    [SerializeField] private float backgroundMinSpacing = 25f;
    [SerializeField] private float backgroundScaleMin = 1.5f;
    [SerializeField] private float backgroundScaleMax = 2.5f;
    [SerializeField] private float backgroundClearance = 12f;
    [SerializeField] private bool backgroundRandomRotation = true;

    private const string RootObjectName = "WildWestEnvironment";
    private const string CliffsRootName = "Cliffs";
    private const string RocksRootName = "Rocks";
    private const string TreesRootName = "Trees";
    private const string BushesRootName = "Bushes";
    private const string GrassRootName = "Grass";
    private const string FencesRootName = "Fences";
    private const string PropsRootName = "Props";
    private const string LanternsRootName = "Lanterns";
    private const string WagonsRootName = "Wagons";
    private const string BackgroundRootName = "Background";

    private static readonly string[] DefaultCliffPrefabPaths =
    {
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_01.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_02.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_03.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_04.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_05.prefab"
    };

    private static readonly string[] DefaultRockPrefabPaths =
    {
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Forest/Prefabs/UNS_Standard_Rock_01.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Forest/Prefabs/UNS_Standard_Rock_02.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Forest/Prefabs/UNS_Standard_Rock_03.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Forest/Prefabs/UNS_Standard_Rock_04.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Forest/Prefabs/UNS_Standard_Rock_05.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/River/Prefabs/UNS_Tiny_Rock_01.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/River/Prefabs/UNS_Tiny_Rock_02.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/River/Prefabs/UNS_Tiny_Rock_03.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/River/Prefabs/UNS_Tiny_Rock_04.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/River/Prefabs/UNS_Tiny_Rock_05.prefab"
    };

    private static readonly string[] DefaultTreePrefabPaths =
    {
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Trees/Fir/Prefabs/UNS_Spruce_01.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Trees/Fir/Prefabs/UNS_Spruce_02.prefab"
    };

    private static readonly string[] DefaultBushPrefabPaths =
    {
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Vegetation/Bushes/Prefabs/UNS_Bush.prefab"
    };

    private static readonly string[] DefaultGrassPrefabPaths =
    {
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Vegetation/Grass/Prefabs/UNS_Grass.prefab"
    };

    private static readonly string[] DefaultFencePrefabPaths =
    {
        "Assets/Prefabs/Environment/Fence_Western_Wooden.prefab"
    };

    private static readonly string[] DefaultPropPrefabPaths =
    {
        "Assets/Prefabs/Environment/Barrel_Western_Wooden.prefab",
        "Assets/Prefabs/Environment/Crate_Western_Wooden.prefab",
        "Assets/Prefabs/Environment/Cover_Western_Wooden.prefab"
    };

    private static readonly string[] DefaultLanternPrefabPaths =
    {
        "Assets/Prefabs/Environment/Lantern_Post_Western.prefab"
    };

    private static readonly string[] DefaultWagonPrefabPaths =
    {
        "Assets/Prefabs/Environment/Wagon_Western_Wooden.prefab"
    };

    private static readonly string[] DefaultBackgroundPrefabPaths =
    {
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Mountain/Prefabs/UNS_Mountain.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_01.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_02.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_03.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_04.prefab",
        "Assets/InnerverseInteractive/Ultimate Nature – Starter/Environment/Rocks/Cliffs/Prefabs/UNS_Rock_Cliff_05.prefab"
    };

    private struct PlacedScatter
    {
        public Vector3 position;
        public float spacing;
    }

    private System.Random rng;
    private Transform environmentRoot;
    private Bounds arenaBounds;
    private bool hasArenaBounds;

    private readonly List<Vector3> exitKeepClearPoints =
        new List<Vector3>();

    private readonly List<PlacedScatter> placedScatters =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedTrees =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedBushes =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedGrass =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedFences =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedProps =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedLanterns =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedWagons =
        new List<PlacedScatter>();

    private readonly List<PlacedScatter> placedBackground =
        new List<PlacedScatter>();

    private Coroutine waitForMapRoutine;

    private int dbgGroundCount;
    private int dbgCliffCount;
    private int dbgRockCount;
    private int dbgCliffRejected;
    private int dbgRockRejected;
    private int dbgTreesPlaced;
    private int dbgTreesRejected;
    private int dbgBushesPlaced;
    private int dbgBushesRejected;
    private int dbgGrassPlaced;
    private int dbgGrassRejected;
    private int dbgFencesPlaced;
    private int dbgFencesRejected;
    private int dbgPropsPlaced;
    private int dbgPropsRejected;
    private int dbgLanternsPlaced;
    private int dbgLanternsRejected;
    private int dbgWagonsPlaced;
    private int dbgWagonsRejected;
    private int dbgBackgroundPlaced;
    private int dbgBackgroundRejected;

    private void Start()
    {
        if (autoGenerate)
        {
            GenerateEnvironment();
        }
    }

    public void GenerateEnvironment()
    {
        Debug.Log(
            "[ENV DIAG] GenerateEnvironment ENTER " +
            "autoGenerate=" + autoGenerate + " " +
            "mapInstance=" + (ArenaTacticalMap.Instance != null) + " " +
            "mapBuilt=" +
            (ArenaTacticalMap.Instance != null &&
                ArenaTacticalMap.Instance.IsBuilt)
        );

        if (ArenaTacticalMap.Instance == null ||
            !ArenaTacticalMap.Instance.IsBuilt)
        {
            WaitForMapReady();
            return;
        }

        PrepareFromMap();
        ClearEnvironment();
        EnsureRoot();
        BuildGround();
        Debug.Log("[ENV DIAG] BuildCliffs START");
        BuildCliffs();
        BuildRocks();
        BuildTrees();
        BuildBushes();
        BuildGrass();
        BuildFences();
        BuildProps();
        BuildLanterns();
        BuildWagons();
        BuildBackground();

        Debug.Log(
            "[ENV DIAG] GenerateEnvironment DONE " +
            "ground=" + dbgGroundCount + " " +
            "cliffs=" + dbgCliffCount + "(" + dbgCliffRejected + " rej) " +
            "rocks=" + dbgRockCount + "(" + dbgRockRejected + " rej) " +
            "trees=" + dbgTreesPlaced + "(" + dbgTreesRejected + " rej) " +
            "bushes=" + dbgBushesPlaced + "(" + dbgBushesRejected + " rej) " +
            "grass=" + dbgGrassPlaced + "(" + dbgGrassRejected + " rej) " +
            "fences=" + dbgFencesPlaced + "(" + dbgFencesRejected + " rej) " +
            "props=" + dbgPropsPlaced + "(" + dbgPropsRejected + " rej) " +
            "lanterns=" + dbgLanternsPlaced + "(" + dbgLanternsRejected + " rej) " +
            "wagons=" + dbgWagonsPlaced + "(" + dbgWagonsRejected + " rej) " +
            "background=" + dbgBackgroundPlaced + "(" + dbgBackgroundRejected + " rej)"
        );
    }

    public void ClearEnvironment()
    {
        if (environmentRoot == null)
        {
            return;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            DestroyImmediate(environmentRoot.gameObject);
        }
        else
        {
            Destroy(environmentRoot.gameObject);
        }
#else
        Destroy(environmentRoot.gameObject);
#endif

        environmentRoot = null;
    }

    private void WaitForMapReady()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (waitForMapRoutine != null)
        {
            StopCoroutine(waitForMapRoutine);
        }

        waitForMapRoutine = StartCoroutine(WaitForMapReadyRoutine());
    }

    private System.Collections.IEnumerator WaitForMapReadyRoutine()
    {
        yield return new WaitUntil(IsMapReady);

        waitForMapRoutine = null;

        GenerateEnvironment();
    }

    private bool IsMapReady()
    {
        return
            ArenaTacticalMap.Instance != null &&
            ArenaTacticalMap.Instance.IsBuilt;
    }

    private void PrepareFromMap()
    {
        int actualSeed = randomizeSeed
            ? System.Environment.TickCount
            : seed;

        rng = new System.Random(actualSeed);

        arenaBounds = new Bounds();
        hasArenaBounds = false;

        exitKeepClearPoints.Clear();
        placedScatters.Clear();
        placedTrees.Clear();
        placedBushes.Clear();
        placedGrass.Clear();
        placedFences.Clear();
        placedProps.Clear();
        placedLanterns.Clear();
        placedWagons.Clear();
        placedBackground.Clear();

        foreach (ArenaTacticalMap.RoomMap room in ArenaTacticalMap.Instance.Rooms)
        {
            if (room == null ||
                room.Module == null)
            {
                continue;
            }

            Bounds roomBounds =
                room.Module.GetWorldBounds();

            if (!hasArenaBounds)
            {
                arenaBounds = roomBounds;
                hasArenaBounds = true;
            }
            else
            {
                arenaBounds.Encapsulate(roomBounds);
            }

            if (room.Exits != null)
            {
                exitKeepClearPoints.AddRange(room.Exits);
            }
        }

        if (hasArenaBounds)
        {
            arenaBounds.Expand(additionalBoundsMargin);
        }
    }

    private void EnsureRoot()
    {
        if (environmentRoot != null)
        {
            return;
        }

        GameObject rootObject = new GameObject(RootObjectName);

        environmentRoot = rootObject.transform;
    }

    private void BuildGround()
    {
        dbgGroundCount = 0;

        if (!groundEnabled || !hasArenaBounds)
        {
            return;
        }

        GameObject groundObject =
            GameObject.CreatePrimitive(PrimitiveType.Plane);

        groundObject.name = "Ground";

        groundObject.transform.SetParent(environmentRoot);

        float sizeX =
            arenaBounds.size.x + groundExtraMargin * 2f;
        float sizeZ =
            arenaBounds.size.z + groundExtraMargin * 2f;

        groundObject.transform.position =
            new Vector3(
                arenaBounds.center.x,
                groundHeight,
                arenaBounds.center.z
            );

        groundObject.transform.localScale =
            new Vector3(
                sizeX / 10f,
                1f,
                sizeZ / 10f
            );

        Material material = ResolveGroundMaterial();

        if (material != null)
        {
            groundObject.GetComponent<MeshRenderer>().sharedMaterial =
                material;
        }

        dbgGroundCount = 1;
    }

    private Material ResolveGroundMaterial()
    {
        if (groundMaterial != null)
        {
            return groundMaterial;
        }

#if UNITY_EDITOR
        Material editorMaterial =
            UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(
                "Assets/Materials/M_Ground_Desert.mat"
            );

        if (editorMaterial != null)
        {
            return editorMaterial;
        }
#endif

        Shader litShader =
            Shader.Find("Universal Render Pipeline/Lit");

        if (litShader == null)
        {
            Debug.LogWarning(
                "WildWestEnvironmentGenerator: ground material is not assigned " +
                "and URP Lit shader was not found."
            );

            return null;
        }

        Material fallbackMaterial = new Material(litShader);

        fallbackMaterial.color =
            new Color(0.42f, 0.28f, 0.14f);

        return fallbackMaterial;
    }

    private void BuildCliffs()
    {
        dbgCliffCount = 0;

        if (!cliffsEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildCliffs BEGIN " +
            "enabled=" + cliffsEnabled + " " +
            "configuredPrefabs=" +
            (cliffPrefabs != null ? cliffPrefabs.Length : -1) + " " +
            "maxCount=" + cliffMaxCount + " " +
            "dist=[" + cliffMinDistance + "," + cliffMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(CliffsRootName);

        Debug.Log(
            "[ENV DIAG] Cliffs root name=" + parent.name + " " +
            "parent=" + parent.parent.name + " " +
            "activeSelf=" + parent.gameObject.activeSelf
        );

        List<GameObject> prefabs =
            ResolvePrefabs(cliffPrefabs, DefaultCliffPrefabPaths);

        dbgCliffCount = PlaceScatter(
            parent,
            prefabs,
            cliffMinDistance,
            cliffMaxDistance,
            cliffDensity,
            cliffMaxCount,
            cliffMinSpacing,
            cliffScaleMin,
            cliffScaleMax,
            cliffClearance,
            cliffRandomRotation,
            0f,
            out dbgCliffRejected,
            false,
            placedScatters,
            false
        );

        Debug.Log("[ENV DIAG] BuildCliffs END placed=" + dbgCliffCount);
    }

    private void BuildRocks()
    {
        dbgRockCount = 0;

        if (!rocksEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildRocks BEGIN " +
            "enabled=" + rocksEnabled + " " +
            "configuredPrefabs=" +
            (rockPrefabs != null ? rockPrefabs.Length : -1) + " " +
            "maxCount=" + rockMaxCount + " " +
            "dist=[" + rockMinDistance + "," + rockMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(RocksRootName);

        Debug.Log(
            "[ENV DIAG] Rocks root name=" + parent.name + " " +
            "parent=" + parent.parent.name + " " +
            "activeSelf=" + parent.gameObject.activeSelf
        );

        List<GameObject> prefabs =
            ResolvePrefabs(rockPrefabs, DefaultRockPrefabPaths);

        dbgRockCount = PlaceScatter(
            parent,
            prefabs,
            rockMinDistance,
            rockMaxDistance,
            rockDensity,
            rockMaxCount,
            rockMinSpacing,
            rockScaleMin,
            rockScaleMax,
            rockClearance,
            rockRandomRotation,
            0f,
            out dbgRockRejected,
            false,
            placedScatters,
            false
        );

        Debug.Log("[ENV DIAG] BuildRocks END placed=" + dbgRockCount);
    }

    private void BuildTrees()
    {
        dbgTreesPlaced = 0;
        dbgTreesRejected = 0;

        if (!treesEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildTrees BEGIN " +
            "enabled=" + treesEnabled + " " +
            "configuredPrefabs=" +
            (treePrefabs != null ? treePrefabs.Length : -1) + " " +
            "maxCount=" + treeMaxCount + " " +
            "dist=[" + treeMinDistance + "," + treeMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(TreesRootName);

        Debug.Log(
            "[ENV DIAG] Trees root name=" + parent.name + " " +
            "parent=" + parent.parent.name + " " +
            "activeSelf=" + parent.gameObject.activeSelf
        );

        List<GameObject> prefabs =
            ResolvePrefabs(treePrefabs, DefaultTreePrefabPaths);

        dbgTreesPlaced = PlaceScatter(
            parent,
            prefabs,
            treeMinDistance,
            treeMaxDistance,
            treeDensity,
            treeMaxCount,
            treeMinSpacing,
            treeScaleMin,
            treeScaleMax,
            treeClearance,
            treeRandomRotation,
            groundHeight,
            out dbgTreesRejected,
            true,
            placedTrees,
            true
        );

        Debug.Log(
            "[ENV DIAG] BuildTrees END placed=" + dbgTreesPlaced + " " +
            "rejected=" + dbgTreesRejected
        );
    }

    private void BuildBushes()
    {
        dbgBushesPlaced = 0;
        dbgBushesRejected = 0;

        if (!bushesEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildBushes BEGIN " +
            "enabled=" + bushesEnabled + " " +
            "configuredPrefabs=" +
            (bushPrefabs != null ? bushPrefabs.Length : -1) + " " +
            "maxCount=" + bushMaxCount + " " +
            "dist=[" + bushMinDistance + "," + bushMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(BushesRootName);

        Debug.Log(
            "[ENV DIAG] Bushes root name=" + parent.name + " " +
            "parent=" + parent.parent.name + " " +
            "activeSelf=" + parent.gameObject.activeSelf
        );

        List<GameObject> prefabs =
            ResolvePrefabs(bushPrefabs, DefaultBushPrefabPaths);

        dbgBushesPlaced = PlaceScatter(
            parent,
            prefabs,
            bushMinDistance,
            bushMaxDistance,
            bushDensity,
            bushMaxCount,
            bushMinSpacing,
            bushScaleMin,
            bushScaleMax,
            bushClearance,
            bushRandomRotation,
            groundHeight,
            out dbgBushesRejected,
            true,
            placedBushes,
            true
        );

        Debug.Log(
            "[ENV DIAG] BuildBushes END placed=" + dbgBushesPlaced + " " +
            "rejected=" + dbgBushesRejected
        );
    }

    private void BuildGrass()
    {
        dbgGrassPlaced = 0;
        dbgGrassRejected = 0;

        if (!grassEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildGrass BEGIN " +
            "enabled=" + grassEnabled + " " +
            "configuredPrefabs=" +
            (grassPrefabs != null ? grassPrefabs.Length : -1) + " " +
            "maxCount=" + grassMaxCount + " " +
            "dist=[" + grassMinDistance + "," + grassMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(GrassRootName);

        Debug.Log(
            "[ENV DIAG] Grass root name=" + parent.name + " " +
            "parent=" + parent.parent.name + " " +
            "activeSelf=" + parent.gameObject.activeSelf
        );

        List<GameObject> prefabs =
            ResolvePrefabs(grassPrefabs, DefaultGrassPrefabPaths);

        dbgGrassPlaced = PlaceScatter(
            parent,
            prefabs,
            grassMinDistance,
            grassMaxDistance,
            grassDensity,
            grassMaxCount,
            grassMinSpacing,
            grassScaleMin,
            grassScaleMax,
            grassClearance,
            grassRandomRotation,
            groundHeight,
            out dbgGrassRejected,
            true,
            placedGrass,
            true
        );

        int grassCollidersDisabled = 0;

        foreach (Transform child in parent)
        {
            if (child == null)
            {
                continue;
            }

            Collider[] colliders =
                child.GetComponentsInChildren<Collider>(true);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null &&
                    colliders[i].enabled)
                {
                    colliders[i].enabled = false;
                    grassCollidersDisabled++;
                }
            }
        }

        Debug.Log(
            "[ENV DIAG] BuildGrass END placed=" + dbgGrassPlaced + " " +
            "rejected=" + dbgGrassRejected + " " +
            "collidersDisabled=" + grassCollidersDisabled
        );
    }

    private void BuildFences()
    {
        dbgFencesPlaced = 0;
        dbgFencesRejected = 0;

        if (!fencesEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildFences BEGIN " +
            "enabled=" + fencesEnabled + " " +
            "configuredPrefabs=" +
            (fencePrefabs != null ? fencePrefabs.Length : -1) + " " +
            "maxCount=" + fenceMaxCount + " " +
            "dist=[" + fenceMinDistance + "," + fenceMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(FencesRootName);

        List<GameObject> prefabs =
            ResolvePrefabs(fencePrefabs, DefaultFencePrefabPaths);

        dbgFencesPlaced = PlaceScatter(
            parent,
            prefabs,
            fenceMinDistance,
            fenceMaxDistance,
            fenceDensity,
            fenceMaxCount,
            fenceMinSpacing,
            fenceScaleMin,
            fenceScaleMax,
            fenceClearance,
            fenceRandomRotation,
            0f,
            out dbgFencesRejected,
            true,
            placedFences,
            true
        );

        Debug.Log(
            "[ENV DIAG] BuildFences END placed=" + dbgFencesPlaced + " " +
            "rejected=" + dbgFencesRejected
        );
    }

    private void BuildProps()
    {
        dbgPropsPlaced = 0;
        dbgPropsRejected = 0;

        if (!propsEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildProps BEGIN " +
            "enabled=" + propsEnabled + " " +
            "configuredPrefabs=" +
            (propPrefabs != null ? propPrefabs.Length : -1) + " " +
            "maxCount=" + propMaxCount + " " +
            "dist=[" + propMinDistance + "," + propMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(PropsRootName);

        List<GameObject> prefabs =
            ResolvePrefabs(propPrefabs, DefaultPropPrefabPaths);

        dbgPropsPlaced = PlaceScatter(
            parent,
            prefabs,
            propMinDistance,
            propMaxDistance,
            propDensity,
            propMaxCount,
            propMinSpacing,
            propScaleMin,
            propScaleMax,
            propClearance,
            propRandomRotation,
            0f,
            out dbgPropsRejected,
            true,
            placedProps,
            true
        );

        Debug.Log(
            "[ENV DIAG] BuildProps END placed=" + dbgPropsPlaced + " " +
            "rejected=" + dbgPropsRejected
        );
    }

    private void BuildLanterns()
    {
        dbgLanternsPlaced = 0;
        dbgLanternsRejected = 0;

        if (!lanternsEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildLanterns BEGIN " +
            "enabled=" + lanternsEnabled + " " +
            "configuredPrefabs=" +
            (lanternPrefabs != null ? lanternPrefabs.Length : -1) + " " +
            "maxCount=" + lanternMaxCount + " " +
            "dist=[" + lanternMinDistance + "," + lanternMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(LanternsRootName);

        List<GameObject> prefabs =
            ResolvePrefabs(lanternPrefabs, DefaultLanternPrefabPaths);

        dbgLanternsPlaced = PlaceScatter(
            parent,
            prefabs,
            lanternMinDistance,
            lanternMaxDistance,
            lanternDensity,
            lanternMaxCount,
            lanternMinSpacing,
            lanternScaleMin,
            lanternScaleMax,
            lanternClearance,
            lanternRandomRotation,
            0f,
            out dbgLanternsRejected,
            true,
            placedLanterns,
            true
        );

        Debug.Log(
            "[ENV DIAG] BuildLanterns END placed=" + dbgLanternsPlaced + " " +
            "rejected=" + dbgLanternsRejected
        );
    }

    private void BuildWagons()
    {
        dbgWagonsPlaced = 0;
        dbgWagonsRejected = 0;

        if (!wagonsEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildWagons BEGIN " +
            "enabled=" + wagonsEnabled + " " +
            "configuredPrefabs=" +
            (wagonPrefabs != null ? wagonPrefabs.Length : -1) + " " +
            "maxCount=" + wagonMaxCount + " " +
            "dist=[" + wagonMinDistance + "," + wagonMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(WagonsRootName);

        List<GameObject> prefabs =
            ResolvePrefabs(wagonPrefabs, DefaultWagonPrefabPaths);

        dbgWagonsPlaced = PlaceScatter(
            parent,
            prefabs,
            wagonMinDistance,
            wagonMaxDistance,
            wagonDensity,
            wagonMaxCount,
            wagonMinSpacing,
            wagonScaleMin,
            wagonScaleMax,
            wagonClearance,
            wagonRandomRotation,
            0f,
            out dbgWagonsRejected,
            true,
            placedWagons,
            true
        );

        Debug.Log(
            "[ENV DIAG] BuildWagons END placed=" + dbgWagonsPlaced + " " +
            "rejected=" + dbgWagonsRejected
        );
    }

    private void BuildBackground()
    {
        dbgBackgroundPlaced = 0;
        dbgBackgroundRejected = 0;

        if (!backgroundEnabled || !hasArenaBounds)
        {
            return;
        }

        Debug.Log(
            "[ENV DIAG] BuildBackground BEGIN " +
            "enabled=" + backgroundEnabled + " " +
            "configuredPrefabs=" +
            (backgroundPrefabs != null ? backgroundPrefabs.Length : -1) + " " +
            "maxCount=" + backgroundMaxCount + " " +
            "dist=[" + backgroundMinDistance + "," + backgroundMaxDistance + "]"
        );

        Transform parent = EnsureCategoryRoot(BackgroundRootName);

        Debug.Log(
            "[ENV DIAG] Background root name=" + parent.name + " " +
            "parent=" + parent.parent.name + " " +
            "activeSelf=" + parent.gameObject.activeSelf
        );

        List<GameObject> prefabs =
            ResolvePrefabs(backgroundPrefabs, DefaultBackgroundPrefabPaths);

        dbgBackgroundPlaced = PlaceScatter(
            parent,
            prefabs,
            backgroundMinDistance,
            backgroundMaxDistance,
            backgroundDensity,
            backgroundMaxCount,
            backgroundMinSpacing,
            backgroundScaleMin,
            backgroundScaleMax,
            backgroundClearance,
            backgroundRandomRotation,
            groundHeight,
            out dbgBackgroundRejected,
            true,
            placedBackground,
            true
        );

        int backgroundCollidersDisabled = 0;

        foreach (Transform child in parent)
        {
            if (child == null)
            {
                continue;
            }

            Collider[] colliders =
                child.GetComponentsInChildren<Collider>(true);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null &&
                    colliders[i].enabled)
                {
                    colliders[i].enabled = false;
                    backgroundCollidersDisabled++;
                }
            }
        }

        Debug.Log(
            "[ENV DIAG] BuildBackground END placed=" + dbgBackgroundPlaced + " " +
            "rejected=" + dbgBackgroundRejected + " " +
            "collidersDisabled=" + backgroundCollidersDisabled
        );
    }

    private Transform EnsureCategoryRoot(string categoryName)
    {
        GameObject categoryObject = new GameObject(categoryName);

        categoryObject.transform.SetParent(environmentRoot);

        return categoryObject.transform;
    }

    private List<GameObject> ResolvePrefabs(
        GameObject[] configured,
        string[] knownPaths)
    {
        List<GameObject> result = new List<GameObject>();

        if (configured != null)
        {
            for (int i = 0; i < configured.Length; i++)
            {
                if (configured[i] != null)
                {
                    result.Add(configured[i]);
                }
            }
        }

        if (result.Count > 0)
        {
            return result;
        }

#if UNITY_EDITOR
        if (knownPaths != null)
        {
            for (int i = 0; i < knownPaths.Length; i++)
            {
                GameObject prefab =
                    UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(
                        knownPaths[i]
                    );

                if (prefab != null)
                {
                    result.Add(prefab);
                }
            }
        }
#endif

        return result;
    }

    private int PlaceScatter(
        Transform parent,
        List<GameObject> prefabs,
        float minDistance,
        float maxDistance,
        float density,
        int maxCount,
        float minSpacing,
        float scaleMin,
        float scaleMax,
        float clearance,
        bool randomRotation,
        float baseY,
        out int rejectedCount,
        bool sampleFromEdge,
        List<PlacedScatter> spacingList,
        bool checkLargeObjects)
    {
        rejectedCount = 0;

        if (parent == null ||
            prefabs == null ||
            prefabs.Count == 0 ||
            maxCount <= 0)
        {
            return 0;
        }

        float clampedDensity = Mathf.Clamp01(density);

        int targetCount = Mathf.RoundToInt(maxCount * clampedDensity);

        if (targetCount <= 0)
        {
            return 0;
        }

        if (maxDistance < minDistance)
        {
            maxDistance = minDistance;
        }

        if (scaleMax < scaleMin)
        {
            scaleMax = scaleMin;
        }

        Bounds keepOutBounds = arenaBounds;
        keepOutBounds.Expand(clearance);

        int attempts = targetCount * 10 + 16;
        int placed = 0;
        int iterations = 0;

        for (int attempt = 0; attempt < attempts && placed < targetCount; attempt++)
        {
            iterations++;
            GameObject prefab = prefabs[rng.Next(prefabs.Count)];

            if (prefab == null)
            {
                continue;
            }

            float angle =
                (float)(rng.NextDouble() * System.Math.PI * 2.0);

            float dirX = Mathf.Cos(angle);
            float dirZ = Mathf.Sin(angle);

            float bandOffset =
                minDistance +
                (float)rng.NextDouble() * (maxDistance - minDistance);

            float baseRadius = 0f;

            if (sampleFromEdge)
            {
                float edgeDistance = float.MaxValue;
                float halfX = arenaBounds.extents.x;
                float halfZ = arenaBounds.extents.z;

                if (Mathf.Abs(dirX) > 0.00001f)
                {
                    edgeDistance = Mathf.Min(
                        edgeDistance,
                        halfX / Mathf.Abs(dirX));
                }

                if (Mathf.Abs(dirZ) > 0.00001f)
                {
                    edgeDistance = Mathf.Min(
                        edgeDistance,
                        halfZ / Mathf.Abs(dirZ));
                }

                if (edgeDistance == float.MaxValue)
                {
                    continue;
                }

                baseRadius = edgeDistance;
            }

            float distance = baseRadius + bandOffset;

            Vector3 point = new Vector3(
                arenaBounds.center.x + dirX * distance,
                0f,
                arenaBounds.center.z + dirZ * distance
            );

            if (keepOutBounds.Contains(point))
            {
                continue;
            }

            bool exitBlocked = false;

            for (int i = 0; i < exitKeepClearPoints.Count; i++)
            {
                Vector3 exit = exitKeepClearPoints[i];

                float dx = point.x - exit.x;
                float dz = point.z - exit.z;

                if (dx * dx + dz * dz <
                    exitKeepClearRadius * exitKeepClearRadius)
                {
                    exitBlocked = true;
                    break;
                }
            }

            if (exitBlocked)
            {
                continue;
            }

            bool tooClose = IsTooCloseToList(
                point,
                spacingList,
                minSpacing
            );

            if (!tooClose && checkLargeObjects)
            {
                tooClose = IsTooCloseToList(
                    point,
                    placedScatters,
                    minSpacing
                );
            }

            if (tooClose)
            {
                continue;
            }

            float scale =
                scaleMin +
                (float)rng.NextDouble() * (scaleMax - scaleMin);

            float rotationY = randomRotation
                ? (float)rng.NextDouble() * 360f
                : 0f;

            Debug.Log(
                "[ENV DIAG] Place n=" + (placed + 1) + " " +
                "prefab=" + prefab.name + " " +
                "pos=(" + point.x.ToString("F1") + "," + point.z.ToString("F1") + ")"
            );

            GameObject instance = Instantiate(
                prefab,
                new Vector3(point.x, baseY, point.z),
                Quaternion.Euler(0f, rotationY, 0f),
                parent
            );

            // Пропы с укрытиями (забор и будущие) сами несут CoverPoint:
            // регистрируем в существующем CoverSystem без его переделки.
            CoverPoint[] instanceCoverPoints =
                instance.GetComponentsInChildren<CoverPoint>(true);

            if (instanceCoverPoints != null &&
                instanceCoverPoints.Length > 0 &&
                CoverSystem.Instance != null)
            {
                CoverSystem.Instance.RegisterCoverPoints(instanceCoverPoints);
            }

            instance.transform.localScale =
                new Vector3(scale, scale, scale);

            instance.isStatic = true;

            PlacedScatter accepted;
            accepted.position = instance.transform.position;
            accepted.spacing = minSpacing;

            if (spacingList != null)
            {
                spacingList.Add(accepted);
            }

            placed++;
        }

        rejectedCount = iterations - placed;

        return placed;
    }

    private bool IsTooCloseToList(
        Vector3 point,
        List<PlacedScatter> list,
        float minSpacing)
    {
        if (list == null)
        {
            return false;
        }

        for (int i = 0; i < list.Count; i++)
        {
            PlacedScatter placedScatter = list[i];

            float allowedSpacing = Mathf.Max(
                minSpacing,
                placedScatter.spacing
            );

            float dx = point.x - placedScatter.position.x;
            float dz = point.z - placedScatter.position.z;

            if (dx * dx + dz * dz < allowedSpacing * allowedSpacing)
            {
                return true;
            }
        }

        return false;
    }
}
