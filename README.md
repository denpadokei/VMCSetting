# VMCSetting


Beat Saber の中から VMCProtocol でキャリブレーションを要求する MOD です。三人称視点の鏡プレビューが付いているので、kawaii ムーブの練習にも使えます。

![vmpc_logo_128x128](https://user-images.githubusercontent.com/55026301/99907665-6c5ae900-2d21-11eb-81f0-b91c8d1d5da9.png)

## 機能

- メインメニューの `VMC SETTING` ボタンから設定画面を開ける
- キャリブレーションモードを「1 Normal」「2 Floor correction」から選択
- `Calibrate` ボタンで 5 秒のカウントダウン後にキャリブレーションを実行
- OSC で `/VMC/Ext/Set/Calib/Ready` と `/VMC/Ext/Set/Calib/Exec` を `127.0.0.1:39540` へ送信
- 設定画面の背後に三人称視点の鏡プレビューを表示 (一人称専用オブジェクトは映さない)

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.11.1 |
| 依存 MOD | BSIPA 4.0.5 |

## インストール

1. Beat Saber を 1.11.1 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/VMCSetting/releases) から `VMCSetting-0.0.2-bs1.11.1.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

VMC 側で OSC 受信を有効にしておいてください。

1. Beat Saber のメインメニューで `VMC SETTING` を押す
2. `Caliblate mode` でキャリブレーションモードを選ぶ (`1 Normal` / `2 Floor correction`)
3. `Calibrate` を押すとまず `/VMC/Ext/Set/Calib/Ready` を送信して 5 秒のカウントダウンが始まり、カウントダウン後に `/VMC/Ext/Set/Calib/Exec` へモード番号 (1 または 2) を付けて送信する
4. ステータス欄が `Compleate` になれば完了

## ビルド

```powershell
git clone https://github.com/denpadokei/VMCSetting.git
cd VMCSetting
dotnet build .\VMCSetting\VMCSetting.csproj -c Release
```

`BeatSaberDir` に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。


> GitHub Actions によるビルドは未設定です。コードが現在の FlowCoordinator API と一致せず、manifest の対象バージョン (1.11.1) 向けの参照アセンブリではコンパイルできないため。

## ライセンス

MIT
