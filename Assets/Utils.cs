using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Animations;
using UnityEngine.Playables;
using System.Globalization;
using System.Text;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static partial class Utils
{
    // Commons
    static public Transform FindGrandChild(this Transform fromGameObject, string withName, bool includeInactive = true)
    {
        Transform[] ts = fromGameObject.GetComponentsInChildren<Transform>(includeInactive);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t;
        return null;
    }

    //tries to preserve negative stats e.g. 0..3/4 = 0,  4..7/4 = 1,  but -1...-4 = -1, -5...-8 = -2 and so on
    public static int nfdiv(float a, float b)
    {
        return (int)(a > 0 ? a / b : (a - b + 1) / b);
    }
    public static float nfmod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    public static float Damp(float source, float target, float smoothing, float dt)
    {
        return Mathf.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }
    public static Vector3 Damp(this Vector3 source, Vector3 target, float smoothing, float dt)
    {
        return Vector3.Lerp(source, target, 1.0f - Mathf.Pow(smoothing, dt));
    }
    public static Vector4 Damp(this Vector4 source, Vector4 target, float smoothing, float dt)
    {
        return Vector4.Lerp(source, target, 1.0f - Mathf.Pow(smoothing, dt));
    }
    public static Quaternion Damp(this Quaternion source, Quaternion target, float smoothing, float dt)
    {
        return Quaternion.Lerp(source, target, 1.0f - Mathf.Pow(smoothing, dt));
    }

    public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ToUnixTimeSeconds(this DateTime value)
    {
        DateTimeOffset dto = new DateTimeOffset(value);
        return dto.ToUnixTimeSeconds();
    }

    public static long ToUnixTimeMS(this DateTime value)
    {
        DateTimeOffset dto = new DateTimeOffset(value);
        return dto.ToUnixTimeMilliseconds();
    }


    public static bool Is01(this float a)
    {
        return a > 0 && a < 1;
    }

    public static float clerp(float start, float end, float value)
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs((max - min) * 0.5f);
        float retval = 0.0f;
        float diff = 0.0f;
        if ((end - start) < -half)
        {
            diff = ((max - start) + end) * value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start) * value;
            retval = start + diff;
        }
        else retval = start + (end - start) * value;
        return retval;
    }

    public static float Snap(this float value, float interval)
    {
        return Mathf.Round(value / interval) * interval;
    }


    // Vectors
    public static Vector2 xy(this Vector3 v) => new Vector2(v.x, v.y);
    public static Vector2 xz(this Vector3 v) => new Vector2(v.x, v.z);
    public static Vector2 yz(this Vector3 v) => new Vector2(v.y, v.z);
    public static Vector3 xy0(this Vector2 v) => new Vector3(v.x, v.y, 0);
    public static Vector3 xz(this Vector2 v) => new Vector3(v.x, 0, v.y);
    public static Vector3 yz(this Vector2 v) => new Vector3(0, v.x, v.y);
    public static float Max3(this Vector3 v) => Mathf.Max(v.x, v.y, v.z);
    public static float Max2(this Vector2 v) => Mathf.Max(v.x, v.y);
    public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 0)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }

    public static Vector3 RoundMemberwise(this Vector3 src)
    {
        src.x = Mathf.Round(src.x);
        src.y = Mathf.Round(src.y);
        src.z = Mathf.Round(src.z);
        return src;
    }
    public static Vector3 FloorMemberwise(this Vector3 src)
    {
        src.x = Mathf.Floor(src.x);
        src.y = Mathf.Floor(src.y);
        src.z = Mathf.Floor(src.z);
        return src;
    }
    public static Vector3 CeilMemberwise(this Vector3 src)
    {
        src.x = Mathf.Ceil(src.x);
        src.y = Mathf.Ceil(src.y);
        src.z = Mathf.Ceil(src.z);
        return src;
    }
    public static Vector3 AbsMemberwise(this Vector3 src)
    {
        src.x = Mathf.Abs(src.x);
        src.y = Mathf.Abs(src.y);
        src.z = Mathf.Abs(src.z);
        return src;
    }

    public static Vector3 DivideMembers(this Vector3 divident, Vector3 divisor)
    {
        divident.x /= divisor.x;
        divident.y /= divisor.y;
        divident.z /= divisor.z;
        return divident;
    }
    public static Vector3 MultiplyMembers(this Vector3 src, Vector3 mul)
    {
        src.Scale(mul);
        return src;
    }

    // AB based ranges
    public static float RemapRanges(this float v, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (((v - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
    }
    public static double RemapRanges(this double v, double oldMin, double oldMax, double newMin, double newMax)
    {
        return (((v - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
    }
    public static Vector3 RemapBounds(this Vector3 v, Bounds oldBounds, Bounds newBounds)
    {
        return (v - oldBounds.center).MultiplyMembers(newBounds.extents.DivideMembers(oldBounds.extents)) + newBounds.center;
    }

    public static Vector2 RoundMemberwise(this Vector2 src)
    {
        src.x = Mathf.Round(src.x);
        src.y = Mathf.Round(src.y);
        return src;
    }
    public static Vector2 FloorMemberwise(this Vector2 src)
    {
        src.x = Mathf.Floor(src.x);
        src.y = Mathf.Floor(src.y);
        return src;
    }

    public static float SnapAngleDeg(float angle, float increment)
    {
        return Mathf.Round(angle / increment) * increment;
    }

    /// <summary>
    /// Catmull Rom Spline equation estimation
    /// </summary>
    /// <param name="t">0...1, the percentage</param>
    /// <param name="p0">prev point</param>
    /// <param name="p1">current point</param>
    /// <param name="p2">next point</param>
    /// <param name="p3">next+1 point</param>
    /// <returns></returns>
    public static Vector3 CatmullRom(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }

    /// <summary>
    /// Renders a catmull-rom spline from list of control points
    /// </summary>
    /// <param name="controlPoints">List with control points</param>
    /// <param name="loop"></param>
    /// <param name="useLengths">If true, length of segment will be used, res is spacing</param>
    /// <param name="resolution">Resolution. num points between segments unless useLengths is true - in which case, spacing</param>
    /// <returns></returns>
    public static List<Vector3> PathSplineCatmullRom(List<Vector3> controlPoints, bool loop, bool useLengths = false, float resolution = 10)
    {
        // We will iterate this dataset to draw control points;
        List<Vector3> SplineDataset = new List<Vector3>();

        // Prepare data for path spline
        if (loop)
        {
            SplineDataset.Add(controlPoints.Last());
            SplineDataset.AddRange(controlPoints);
            SplineDataset.Add(controlPoints.First());
        }
        else
        {
            SplineDataset.Add(controlPoints.First());
            SplineDataset.AddRange(controlPoints);
            SplineDataset.Add(controlPoints.Last());
        }

        // Prepare result with first control point used as is!
        var res = new List<Vector3>();
        res.Add(SplineDataset[1]);

        // outer loop: Control points
        for (int i = 1; i < SplineDataset.Count - 2; i++)
        {
            int p0 = i - 1;
            int p1 = i + 0;
            int p2 = i + 1;
            int p3 = i + 2;
            float p;
            if (!useLengths) p = 1 / resolution;
            else
            {
                var l = Vector3.Distance(SplineDataset[p1], SplineDataset[p2]);
                p = 1 / l * resolution;
            }

            if (p <= float.Epsilon || float.IsInfinity(p) || float.IsNaN(p))
            {
                res.Add(SplineDataset[p2]); // hang defense, just add next control point
                continue;
            }
            float t = p; // 0 will not be iterated
            while (t < 1)
            {
                res.Add(CatmullRom(t, SplineDataset[p0], SplineDataset[p1], SplineDataset[p2], SplineDataset[p3]));
                t += p;
            }
            res.Add(CatmullRom(1, SplineDataset[p0], SplineDataset[p1], SplineDataset[p2], SplineDataset[p3]));
        }
        return res;
    }


    // Randoms
    public static T Choose<T>(params T[] x) => x[UnityEngine.Random.Range(0, x.Length)];
    public static T Choose<T>(IList<T> x) => x[UnityEngine.Random.Range(0, x.Count)];
    public static int Choose(params int[] x) => x[UnityEngine.Random.Range(0, x.Length)];
    public static float RandomRange(this Vector2 v) => UnityEngine.Random.Range(v.x, v.y);

    public static T GetRandomElement<T>(this IList<T> collection)
    {
        return collection[UnityEngine.Random.Range(0, collection.Count)];
    }

    public static T GetRandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
    {
        float totalWeight = sequence.Sum(weightSelector);
        float itemWeightIndex = UnityEngine.Random.value * totalWeight;
        float currentWeightIndex = 0;
        foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
        {
            currentWeightIndex += item.Weight;
            if (currentWeightIndex >= itemWeightIndex)
                return item.Value;
        }
        return default(T);
    }


    // Closest stuff
    public static Vector3 GetClosestPoint(Vector3 origin, IEnumerable<Vector3> points)
    {
        Vector3 tMin = Vector3.negativeInfinity;
        float minDist = float.PositiveInfinity;
        foreach (var p in points)
        {
            float dist = Vector3.Distance(p, origin);
            if (dist < minDist)
            {
                tMin = p;
                minDist = dist;
            }
        }
        return tMin;
    }

    public static float GetClosestNumber(float origin, IEnumerable<float> points)
    {
        float tMin = float.NegativeInfinity;
        float minDist = float.PositiveInfinity;
        foreach (var p in points)
        {
            float dist = Mathf.Abs(p - origin);
            if (dist < minDist)
            {
                tMin = p;
                minDist = dist;
            }
        }
        return tMin;
    }

    public static Transform GetClosestObject(Vector3 origin, IEnumerable<GameObject> objects, float maxDist = float.PositiveInfinity)
    {
        Transform tMin = null;
        float minDist = float.PositiveInfinity;
        Vector3 currentPos = origin;
        foreach (GameObject t in objects)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist > maxDist) continue;
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    public static Transform GetClosestObject(Vector3 origin, IEnumerable<Transform> objects)
    {
        Transform tMin = null;
        float minDist = float.PositiveInfinity;
        Vector3 currentPos = origin;
        foreach (Transform t in objects)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    public static Transform GetClosestObjectWithTag(Vector3 origin, string tag)
    {
        return GetClosestObject(origin, GameObject.FindGameObjectsWithTag(tag));
    }

    public static T[] GetAllComponents<T>() where T : UnityEngine.Object
    {
        return Resources.FindObjectsOfTypeAll<T>();
    }

    public static Transform GetClosestObjectWithComponent<T>(Vector3 origin) where T : Component
    {
        var objs = UnityEngine.Object.FindObjectsOfType<T>();
        List<GameObject> gameobjects = new List<GameObject>();
        foreach (var c in objs)
        {
            gameobjects.Add(c.gameObject);
        }
        return GetClosestObject(origin, gameobjects.ToArray());
    }

    public static T GetClosestComponent<T>(Vector3 origin) where T : Component
    {
        var objs = UnityEngine.Object.FindObjectsOfType<T>();
        List<GameObject> gameobjects = new List<GameObject>();
        foreach (var c in objs)
        {
            gameobjects.Add(c.gameObject);
        }
        var co = GetClosestObject(origin, gameobjects.ToArray());
        if (co != null) return co.GetComponent<T>();
        else return null;
    }

    public static T GetClosestComponent<T>(Vector3 origin, IEnumerable<T> allowedComponents) where T : Component
    {
        T closestComponent = null;
        float closestDistance = float.MaxValue;
        foreach (var component in allowedComponents)
        {
            if (component == null) continue;
            float distance = Vector3.Distance(origin, component.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestComponent = component;
            }
        }
        return closestComponent;
    }


    public static T GetClosestObjectImplementingInterface<T>(Vector3 origin, float maxDist, List<GameObject> exclude = null) where T : class
    {
        var ss = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().OfType<T>().ToList();       
        var objects = new List<GameObject>();
        foreach (var c in ss)
        {
            GameObject go = (c as MonoBehaviour).gameObject;
            if (exclude == null) objects.Add(go);
            else if (!exclude.Contains(go)) objects.Add(go);
        }
        return GetClosestObject(origin, objects, maxDist)?.GetComponent<T>();
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * 0.5f), size);
    }

    public static T GetOrDefault<T>(this System.Array arr, int index, T def)
    {
        if (index < 0) return def;
        if (index >= arr.Length) return def;
        return (T)arr.GetValue(index);
    }

    public static T GetOrDefault<T>(this List<T> arr, int index, T def)
    {
        if (index < 0) return def;
        if (index >= arr.Count) return def;
        return (T)arr.ToArray().GetValue(index);
    }

    public static V GetOrDefault<K, V>(this Dictionary<K, V> dict, K key, V def)
    {
        if (dict == null) return def;
        V val;
        if (dict.TryGetValue(key, out val)) return val;
        return def;
    }

    public static V GetOrNull<K, V>(this Dictionary<K, V> dict, K key)
    {
        if (dict == null) return default(V);
        V val;
        if (dict.TryGetValue(key, out val)) return val;
        return default(V);
    }

    // Misc
    public static T GetOrAddComponent<T>(this GameObject o) where T : Component
    {
        T x = o.GetComponent<T>();
        if (x != null) return x;
        return o.AddComponent<T>();
    }

    public static bool IsInLayerMask(this int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static void SetLayerRecursively(this GameObject obj, int layer, int maxLevels=100)
    {
        SetLayerRecursivelyInternal(obj, layer, maxLevels, 0);
    }

    private static void SetLayerRecursivelyInternal(GameObject obj, int layer, int maxLevels, int currentLevel)
    {
        if (currentLevel > maxLevels) return;

        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursivelyInternal(child.gameObject, layer, maxLevels, currentLevel + 1);
        }
    }

    public static void Swap<T>(ref T a, ref T b) where T : class
    {
        T c = a;
        a = b;
        b = c;
    }

    public static PlayableGraph CreatePlayableGraph(this AnimationClip clip, GameObject target, DirectorUpdateMode updateMode = DirectorUpdateMode.GameTime)
    {
        PlayableGraph playableGraph = PlayableGraph.Create("Graph-" + clip.name);
        playableGraph.SetTimeUpdateMode(updateMode);
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, clip.name, target.GetComponent<Animator>());
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        playableOutput.SetSourcePlayable(clipPlayable);
        return playableGraph;
    }

    public static int hash32i(string source)
    {
        const int MULTIPLIER = 36;
        int h = 0;
        for (int i = 0; i < source.Length; ++i)
            h = MULTIPLIER * h + source[i];
        return h;
    }

    public static void Each<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T item in source)
            action(item);
    }

    // Reflection
    public static void CopyPublicMembers<T>(T sourceComp, T targetComp)
    {
        FieldInfo[] sourceFields = sourceComp.GetType().GetFields(BindingFlags.Public |
                                                         BindingFlags.NonPublic |
                                                         BindingFlags.Instance);
        int i = 0;
        for (i = 0; i < sourceFields.Length; i++)
        {
            var value = sourceFields[i].GetValue(sourceComp);
            sourceFields[i].SetValue(targetComp, value);
        }
    }

    // Porting Utils
    public static bool IsDeviceWithTouchscreen()
    {
#if UNITY_EDITOR             
        return true; // Force true on Editor
#elif UNITY_IOS || __IOS__ || UNITY_ANDROID || __ANDROID__
        return true;
#elif UNITY_PS4 || __PS4__ || UNITY_PS5 || __PS5__
        return false;
#elif UNITY_GAMECORE || __GAMECORE__
        // GameCore (Xbox platforms) does not have a touchscreen
        return false;
#elif UNITY_SWITCH || __SWITCH__
        TODO: Use nn.hid  [Resolve this compile error!]
        return true if not docked
#else
        // Default to false for unsupported or unknown platforms
        return false;
#endif
    }

    // UNICODE NONSENSE
    static int GetUnicodeCodePoint(string input, ref int index)
    {
        char highSurrogate = input[index];

        // Check if the character is a high surrogate
        if (char.IsHighSurrogate(highSurrogate) && index + 1 < input.Length)
        {
            char lowSurrogate = input[index + 1];
            if (char.IsLowSurrogate(lowSurrogate))
            {
                // Combine high and low surrogate to get the full Unicode code point
                index++; // Advance the index since we consumed two characters
                return 0x10000 + ((highSurrogate - 0xD800) << 10) + (lowSurrogate - 0xDC00);
            }
        }

        // If it's not a surrogate pair, just return the character itself
        return highSurrogate;
    }

    public static string NormalizeLeetText(this string input)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            int codePoint = GetUnicodeCodePoint(input, ref i);

            // Step 1: Check if it's a non-Latin script (like Japanese/Chinese), keep it.
            if (IsMathematicalLetter(codePoint))
            {
                // Step 2: If code point is a mathematical fancy letter, convert to ASCII.
                result.Append(ConvertToAsciiEquivalent(codePoint));
            }
            else if (char.GetUnicodeCategory((char)codePoint) == UnicodeCategory.OtherLetter)
            {
                result.Append(char.ConvertFromUtf32(codePoint));
            }
            else
            {
                // Step 3: Default to just appending the character as it is.
                result.Append(char.ConvertFromUtf32(codePoint));
            }
        }

        return result.ToString();
    }

    // Check if a rune is in the Mathematical Alphanumeric Symbols block
    public static bool IsMathematicalLetter(int c)
    {
        // Unicode ranges for fancy "mathematical letters"
        // Using integer values instead of character literals for Unicode code points
        return (c >= 0x1D400 && c <= 0x1D7FF);
    }

    // Convert to printable equivalent
    public static char ConvertToAsciiEquivalent(int c)
    {
        // Bold A-Z (𝐀 - 𝐙)
        if (c >= 0x1D400 && c <= 0x1D419) return (char)(c - 0x1D400 + 'A');
        // Bold a-z (𝐚 - 𝐳)
        if (c >= 0x1D41A && c <= 0x1D433) return (char)(c - 0x1D41A + 'a');
        // Italic A-Z (𝐴 - 𝑍)
        if (c >= 0x1D434 && c <= 0x1D44D) return (char)(c - 0x1D434 + 'A');
        // Italic a-z (𝑎 - 𝑧)
        if (c >= 0x1D44E && c <= 0x1D467) return (char)(c - 0x1D44E + 'a');
        // Bold Italic A-Z (𝑨 - 𝒁)
        if (c >= 0x1D468 && c <= 0x1D481) return (char)(c - 0x1D468 + 'A');
        // Bold Italic a-z (𝒂 - 𝒛)
        if (c >= 0x1D482 && c <= 0x1D49B) return (char)(c - 0x1D482 + 'a');
        // Script A-Z (𝒜 - 𝒵)
        if (c >= 0x1D49C && c <= 0x1D4B5) return (char)(c - 0x1D49C + 'A');
        // Script a-z (𝒶 - 𝓏)
        if (c >= 0x1D4B6 && c <= 0x1D4CF) return (char)(c - 0x1D4B6 + 'a');
        // Bold Script A-Z (𝓐 - 𝓩)
        if (c >= 0x1D4D0 && c <= 0x1D4E9) return (char)(c - 0x1D4D0 + 'A');
        // Bold Script a-z (𝓪 - 𝔃)
        if (c >= 0x1D4EA && c <= 0x1D503) return (char)(c - 0x1D4EA + 'a');
        // Fraktur A-Z (𝔄 - 𝔜)
        if (c >= 0x1D504 && c <= 0x1D51C) return (char)(c - 0x1D504 + 'A');
        // Fraktur a-z (𝔞 - 𝔷)
        if (c >= 0x1D51E && c <= 0x1D537) return (char)(c - 0x1D51E + 'a');
        // Double-struck A-Z (𝔸 - 𝕐)
        if (c >= 0x1D538 && c <= 0x1D551) return (char)(c - 0x1D538 + 'A');
        // Double-struck a-z (𝕒 - 𝕫)
        if (c >= 0x1D552 && c <= 0x1D56B) return (char)(c - 0x1D552 + 'a');
        // Bold Fraktur A-Z (𝕬 - 𝖅)
        if (c >= 0x1D56C && c <= 0x1D585) return (char)(c - 0x1D56C + 'A');
        // Bold Fraktur a-z (𝖆 - 𝖟)
        if (c >= 0x1D586 && c <= 0x1D59F) return (char)(c - 0x1D586 + 'a');
        // Sans-serif A-Z (𝖠 - 𝖹)
        if (c >= 0x1D5A0 && c <= 0x1D5B9) return (char)(c - 0x1D5A0 + 'A');
        // Sans-serif a-z (𝖺 - 𝗓)
        if (c >= 0x1D5BA && c <= 0x1D5D3) return (char)(c - 0x1D5BA + 'a');
        // Sans-serif Bold A-Z (𝗔 - 𝗭)
        if (c >= 0x1D5D4 && c <= 0x1D5ED) return (char)(c - 0x1D5D4 + 'A');
        // Sans-serif Bold a-z (𝗮 - 𝘇)
        if (c >= 0x1D5EE && c <= 0x1D607) return (char)(c - 0x1D5EE + 'a');
        // Sans-serif Italic A-Z (𝘈 - 𝘡)
        if (c >= 0x1D608 && c <= 0x1D621) return (char)(c - 0x1D608 + 'A');
        // Sans-serif Italic a-z (𝘢 - 𝘻)
        if (c >= 0x1D622 && c <= 0x1D63B) return (char)(c - 0x1D622 + 'a');
        // Sans-serif Bold Italic A-Z (𝘼 - 𝙕)
        if (c >= 0x1D63C && c <= 0x1D655) return (char)(c - 0x1D63C + 'A');
        // Sans-serif Bold Italic a-z (𝙖 - 𝙯)
        if (c >= 0x1D656 && c <= 0x1D66F) return (char)(c - 0x1D656 + 'a');
        // Monospace A-Z (𝙰 - 𝚉)
        if (c >= 0x1D670 && c <= 0x1D689) return (char)(c - 0x1D670 + 'A');
        // Monospace a-z (𝚊 - 𝚣)
        if (c >= 0x1D68A && c <= 0x1D6A3) return (char)(c - 0x1D68A + 'a');
        // Monospace 0-9 (𝟶 - 𝟿)
        if (c >= 0x1D7F6 && c <= 0x1D7FF) return (char)(c - 0x1D7F6 + '0');

        // Default return the original character if no mapping exists
        return (char)c;
    }

    // Reference Wrapper 
    public sealed class Ref<T> where T : struct
    {
        public Ref(T value) => Value = value;
        public T Value;
        public static implicit operator T(Ref<T> value) => value.Value;
        public override string ToString() => Value.ToString();
    }

    // Weighted Random
    [Serializable]
    public class Weighted<T>
    {
        public T item;
        public float weight;
    }
    public static T GetByWeight<T>(this ICollection<Weighted<T>> collection)
    {
        return collection.GetRandomElementByWeight(x => x.weight).item;
    }


}