#if UNITY_4_0||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
#define UNITY_4
#endif

using UnityEngine;

public partial class TOD_Sky : MonoBehaviour
{
	private void Initialize()
	{
		Components = GetComponent<TOD_Components>();
		Components.Initialize();

		Resources = GetComponent<TOD_Resources>();
		Resources.Initialize();

		instances.Add(this);
		Initialized = true;
	}

	private void Cleanup()
	{
		#if !UNITY_4
		if (Probe) Destroy(Probe.gameObject);
		#endif

		instances.Remove(this);
		Initialized = false;
	}

	protected void OnEnable()
	{
		LateUpdate();
	}

	protected void OnDisable()
	{
		Cleanup();
	}

	protected void LateUpdate()
	{
		if (!Initialized) Initialize();

		UnityEngine.Profiling.Profiler.BeginSample("UpdateScattering");
		UpdateScattering();
		UnityEngine.Profiling.Profiler.EndSample();

		UnityEngine.Profiling.Profiler.BeginSample("UpdateCelestials");
		UpdateCelestials();
		UnityEngine.Profiling.Profiler.EndSample();

		UnityEngine.Profiling.Profiler.BeginSample("UpdateQualitySettings");
		UpdateQualitySettings();
		UnityEngine.Profiling.Profiler.EndSample();

		UnityEngine.Profiling.Profiler.BeginSample("UpdateRenderSettings");
		UpdateRenderSettings();
		UnityEngine.Profiling.Profiler.EndSample();

		UnityEngine.Profiling.Profiler.BeginSample("UpdateShaderKeywords");
		UpdateShaderKeywords();
		UnityEngine.Profiling.Profiler.EndSample();

		UnityEngine.Profiling.Profiler.BeginSample("UpdateShaderProperties");
		UpdateShaderProperties();
		UnityEngine.Profiling.Profiler.EndSample();
	}

	protected void OnValidate()
	{
		Cycle.DateTime = Cycle.DateTime;
	}

#if UNITY_EDITOR

	[ContextMenu("Import Parameters")]
	private void EditorImportParameters()
	{
		var folder = UnityEditor.EditorPrefs.GetString("Time of Day Folder", Application.dataPath);
		var path = UnityEditor.EditorUtility.OpenFilePanel("Import", folder, "xml");

		if (string.IsNullOrEmpty(path)) return;

		var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TOD_Parameters));

		using (var filestream = new System.IO.FileStream(path, System.IO.FileMode.Open))
		{
			var reader = new System.Xml.XmlTextReader(filestream);
			var parameters = serializer.Deserialize(reader) as TOD_Parameters;
			parameters.ToSky(this);
			UnityEditor.EditorUtility.SetDirty(this);
		}

		UnityEditor.EditorPrefs.SetString("Time of Day Folder", System.IO.Path.GetDirectoryName(path));
		UnityEditor.EditorPrefs.SetString("Time of Day File", System.IO.Path.GetFileName(path));
	}

	[ContextMenu("Export Parameters")]
	private void EditorExportParameters()
	{
		var folder = UnityEditor.EditorPrefs.GetString("Time of Day Folder", Application.dataPath);
		var file   = UnityEditor.EditorPrefs.GetString("Time of Day File", "Time of Day.xml");
		var path = UnityEditor.EditorUtility.SaveFilePanel("Export", folder, file, "xml");

		if (string.IsNullOrEmpty(path)) return;

		var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TOD_Parameters));

		using (var filestream = new System.IO.FileStream(path, System.IO.FileMode.Create))
		{
			var parameters = new TOD_Parameters(this);
			var writer = new System.Xml.XmlTextWriter(filestream, System.Text.Encoding.UTF8);
			writer.Formatting = System.Xml.Formatting.Indented;
			serializer.Serialize(writer, parameters);
			UnityEditor.AssetDatabase.Refresh();
		}

		UnityEditor.EditorPrefs.SetString("Time of Day Folder", System.IO.Path.GetDirectoryName(path));
		UnityEditor.EditorPrefs.SetString("Time of Day File", System.IO.Path.GetFileName(path));
	}

#endif // UNITY_EDITOR
}
