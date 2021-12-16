using UnityEditor;
using System.IO;

/// <summary>
/// ���ڴ���AB����Ĭ��ʹ��LZ4ѹ���㷨
/// </summary>
public class CreateAssetBundles {
	[MenuItem("Assets/Build AssetBundles/PC")]
	public static void BuildAllAssetBundles() {
		string assetBundleDirectory = "Assets/AssetBundles/PC";//�����ڹ����ļ��е�·��
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/PC");//����һ�������ļ���
        AssetDatabase.Refresh();//ˢ���ļ���
    }

	[MenuItem("Assets/Build AssetBundles/IOS")]
    public static void BuildAllAssetBundles_IOS() {
		string assetBundleDirectory = "Assets/AssetBundles/IOS";//�����ڹ����ļ��е�·��
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/IOS");//����һ�������ļ���
        AssetDatabase.Refresh();//ˢ���ļ���
    }

	[MenuItem("Assets/Build AssetBundles/Android")]
    public static void BuildAllAssetBundles_Android() {
		string assetBundleDirectory = "Assets/AssetBundles/Android";//�����ڹ����ļ��е�·��
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/Android");//����һ�������ļ���
        AssetDatabase.Refresh();//ˢ���ļ���
    }

	[MenuItem("Assets/Build AssetBundles/Other")]
    public static void BuildAllAssetBundles_Other() {
		string assetBundleDirectory = "Assets/AssetBundles/Other";//�����ڹ����ļ��е�·��
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/Other");//����һ�������ļ���
        AssetDatabase.Refresh();//ˢ���ļ���
    }

    //����һ����StreamingAssets�ļ���Ŀ¼��
    private static void CopyAllFiles(string sourceDirectory, string targetDirectory) {
		if (!Directory.Exists(targetDirectory)) {
			Directory.CreateDirectory(targetDirectory);
		}
		try {
			DirectoryInfo dir = new DirectoryInfo(sourceDirectory);
			FileSystemInfo[] fileinfos = dir.GetFileSystemInfos();//��ȡĿ¼�£���������Ŀ¼�����ļ�����Ŀ¼
			foreach (var fileinfo in fileinfos) {
				//�ж��Ƿ����ļ���
				if (fileinfo is DirectoryInfo) {
					if (!Directory.Exists(targetDirectory + "\\" + fileinfo.Name)) {
						Directory.CreateDirectory(targetDirectory + "\\" + fileinfo.Name);//Ŀ��Ŀ¼�²����ڴ��ļ��м��������ļ���
					}
					CopyAllFiles(fileinfo.FullName, targetDirectory + "\\" + fileinfo.Name);//�ݹ���ø������ļ���
				}
				else {
					if (fileinfo.FullName.Contains(".meta")) {
						continue;//�Թ�mate�ļ�
					}
					File.Copy(fileinfo.FullName, targetDirectory + "\\" + fileinfo.Name, true);//�����ļ��м������ļ�
				}
			}
		}
		catch {
			throw;
        }
        AssetDatabase.Refresh();//ˢ���ļ���
    }
}
