using UnityEditor;
using System.IO;

/// <summary>
/// 用于创建AB包，默认使用LZ4压缩算法
/// </summary>
public class CreateAssetBundles {
	[MenuItem("Assets/Build AssetBundles/PC")]
	public static void BuildAllAssetBundles() {
		string assetBundleDirectory = "Assets/AssetBundles/PC";//保存在工程文件夹的路径
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/PC");//复制一份在流文件夹
        AssetDatabase.Refresh();//刷新文件夹
    }

	[MenuItem("Assets/Build AssetBundles/IOS")]
    public static void BuildAllAssetBundles_IOS() {
		string assetBundleDirectory = "Assets/AssetBundles/IOS";//保存在工程文件夹的路径
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/IOS");//复制一份在流文件夹
        AssetDatabase.Refresh();//刷新文件夹
    }

	[MenuItem("Assets/Build AssetBundles/Android")]
    public static void BuildAllAssetBundles_Android() {
		string assetBundleDirectory = "Assets/AssetBundles/Android";//保存在工程文件夹的路径
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/Android");//复制一份在流文件夹
        AssetDatabase.Refresh();//刷新文件夹
    }

	[MenuItem("Assets/Build AssetBundles/Other")]
    public static void BuildAllAssetBundles_Other() {
		string assetBundleDirectory = "Assets/AssetBundles/Other";//保存在工程文件夹的路径
		if (!Directory.Exists(assetBundleDirectory)) {
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory,
			BuildAssetBundleOptions.ChunkBasedCompression,
			BuildTarget.StandaloneWindows);
		CopyAllFiles(assetBundleDirectory, "Assets/StreamingAssets/Other");//复制一份在流文件夹
        AssetDatabase.Refresh();//刷新文件夹
    }

    //复制一份至StreamingAssets文件夹目录内
    private static void CopyAllFiles(string sourceDirectory, string targetDirectory) {
		if (!Directory.Exists(targetDirectory)) {
			Directory.CreateDirectory(targetDirectory);
		}
		try {
			DirectoryInfo dir = new DirectoryInfo(sourceDirectory);
			FileSystemInfo[] fileinfos = dir.GetFileSystemInfos();//获取目录下（不包含子目录）的文件和子目录
			foreach (var fileinfo in fileinfos) {
				//判断是否是文件夹
				if (fileinfo is DirectoryInfo) {
					if (!Directory.Exists(targetDirectory + "\\" + fileinfo.Name)) {
						Directory.CreateDirectory(targetDirectory + "\\" + fileinfo.Name);//目标目录下不存在此文件夹即创建子文件夹
					}
					CopyAllFiles(fileinfo.FullName, targetDirectory + "\\" + fileinfo.Name);//递归调用复制子文件夹
				}
				else {
					if (fileinfo.FullName.Contains(".meta")) {
						continue;//略过mate文件
					}
					File.Copy(fileinfo.FullName, targetDirectory + "\\" + fileinfo.Name, true);//不是文件夹即复制文件
				}
			}
		}
		catch {
			throw;
        }
        AssetDatabase.Refresh();//刷新文件夹
    }
}
