# Kuikkusongu
約100行のプログラムで、ほとんどのメディア再生ソフトウェアから現在再生中のメディア情報を取得します<br>
Spotify、VLC Media Player、YouTube Musicで動作確認済み

## 使い方
ビルド済みのバイナリは[Releases](https://github.com/iiDk-the-actual/QuickSong/releases)タブにあります
```
使用法: Kuikkusongu.exe [オプション]
オプション:
  -namae         : 現在の曲名を表示
  -aatisuto      : 現在のアーティスト名を表示
  -setsumei      : 「タイトル - アーティスト」を表示
  -aikon         : アルバムアートをbase64データ(png)で表示
  -jikan         : 開始時間、終了時間、経過時間をjsonで表示
  -joutai        : 状態を表示: Closed, Opened, Changing, Stopped, Playing, Paused
  -subete        : 全データをjsonで表示
  -? or --herupu : このヘルプメッセージを表示
```
