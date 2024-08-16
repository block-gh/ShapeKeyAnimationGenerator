# ShapeKeyAnimationGenerator
このUnityエディタ拡張は、選択されたオブジェクト（FBXファイルなど）内のすべてのシェイプキーに対して、各シェイプキーを100に設定する1フレームのアニメーションファイルを自動生成するツールです。

VRChat用アバターを作成する際に各シェイプキーを有効にするアニメーションが必要になることが多いため、省力化のため作成しました。
※コードの生成にはClaude 3.5 Sonnetを用いました

# 使用方法
1. Unity エディタで、「Editor」フォルダをAssets内に配置します。
2. 「Tools > Shape Key Animation Generator」からツールウィンドウを開きます。
3. FBXファイルまたはルートオブジェクトを「Target Object」フィールドにドラッグ＆ドロップします。
4. 「Generate Animations」ボタンをクリックし、アニメーションファイルの保存先を選択します。
