This package is compatible with The Built-in Render Pipeline by default. For compatibility with URP, please, import the "URP Assets.unitypackage", included in the project. 

In order to run demoscenes with proper settings you might need to follow next steps:

- Go to Edit > Project Settings > Graphics. In the Scriptable Render Pipeline Settings field assign "URP Asset.asset"

- Go to Edit > Project Settings > Player > Other Settings > Rendering > Color Space and change the value from Gamma to Linear. 

- In newer Unity versions also check for Post Processing in  "URP Asset_Renderer.asset" if Post Processing turned off.



此资源包默认兼容 The Built-in Render Pipeline。若要兼容 URP，请导入项目中包含的 "URP Assets.unitypackage"。

为了正确运行演示场景，您可能需要执行以下步骤：

打开 Edit > Project Settings > Graphics，在 Scriptable Render Pipeline Settings 字段中分配 "URP Asset.asset"。

打开 Edit > Project Settings > Player > Other Settings > Rendering > Color Space，将值从 Gamma 更改为 Linear。

在较新的 Unity 版本中，还需检查 "URP Asset_Renderer.asset" 中的 Post Processing 是否关闭。