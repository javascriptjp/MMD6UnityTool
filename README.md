# MMD6UnityTool
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/K3K1AQ3A3)<br>
### SpecialThanks
> ・Angela<br>
> ・Hanimura<br>
> ・Akatsuki<br>

## このプロジェクトは[MMD6UnityTool](https://github.com/ShiinaRinne/MMD6UnityTool)のフォークです!
#### 元のプロジェクトの制作者に感謝します

## MMD6UnityToolとは

#### `VMD`ファイルからカメラ、モーフアニメーションへ変換するツールです。
#### `PMX`ファイルから`FBX`へ変換したいときは下記のツールをお使いください
### [MMD4Mecanim](https://stereoarts.jp/)

### また下記のプロジェクのコードを参照しています。
※ライセンスなどは元のプロジェクトを参照しお使いください。
> - [MMD2UnityTool](https://github.com/MorphoDiana/MMD2UnityTool)
> - [MMD4UnityTools](https://github.com/ShiinaRinne/MMD4UnityTools)

#### ~~作者曰く2+4=6なのでMMD6らしいです~~

## 使い方

### カメラアニメーションへの変換
  変換したい`VMD`ファイルを右クリックし`VMD Camera To Anim`を選択してください<br>
  アニメーションの構造に基づいてカメラを下記のように配置してください。<br>
  ![](pic/struct.png)
  作成の際に`Hierarchy`にて右クリックし`MMD/Generate MMDCamera`を選択すると<br>
  ↑のオブジェクトを生成することも可能です(下記画像参照)<br>
  ![](pic/camera.png)

  タイプラインでカメラアニメーションを使用する際には<br>
  `Remove Start Offset`のチェックを外してください<br>
  ![](pic/offset.png)

### モーフアニメーションへの変換 
  `VMD`ファイルの上で右クリックをし`VMD Morph To Anim`を選択すると<br>
  `anim`ファイルが生成されます<br>
  またMMD6UnityToolタブの`use 60fps`を選ぶことによって<br>
  `60fps`の`anim`ファイルを生成することも可能です

## あると便利なツール(元製作者様の宣伝)
[TimelineExtensions](https://github.com/ShiinaRinne/TimelineExtensions)<br>
タイムラインからポストプロセスボリュームを弄る事が出来るツール<br>

## License
[MIT License](https://github.com/ShiinaRinne/MMD6UnityTool/blob/master/LICENSE)

## Require
Unity 2021 or later