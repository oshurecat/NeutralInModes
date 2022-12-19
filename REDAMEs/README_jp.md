# Neutral In Modes
![NIMimage](/image/NIMimage.png)

<p align="center"><a href="https://github.com/oshurecat/NeutralInModes/releases/"><img src="https://badgen.net/github/release/oshurecat/NeutralInModes"></a></p>

# このMODについて
これはInnerslothが作ったものじゃなくて個人が作ったものなのでInnerslothは関係ないです。あとこの中にある素材の一部はInnerslothのものです。<br>
また、このMODについてInnerslothに問い合わせるのはお控えください。<br>
 © Innersloth LLC. <br>

 ## Languages/言語

- [日本語](https://github.com/oshurecat/NeutralInModes/blob/main/REDAMEs/README_jp.md) <= here
- [English](https://github.com/oshurecat/NeutralInModes/blob/main/REDAMEs/REDAME_en.md)

# 公式リンク
- [discord](https://discord.gg/FRrEAXJqzx)
- [YouTube](https://www.youtube.com/channel/UCL-TRSrWExcs6ib8PP1bshA)
- [Twitter](https://twitter.com/NIMamongMOD)
- [LINE オープンチャット](https://line.me/ti/g2/63LxmQd-9vvfadFUHfZZ-u0QO_z3OyQoz36fOw?utm_source=invitation&utm_medium=link_copy&utm_campaign=default)

# クレジット
(参考にさせていただきました。ありがとうございます。)
- [BepInEx](https://github.com/BepInEx/BepInEx) ※MODの適応のために使用。
- [TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)※modの作り方や役職のソースコードを参考にさせていただいております。**TheOtherRolesAU**さんありがとうございます！
- [SuperNewRoles](https://github.com/ykundesu/SuperNewRoles) ※modの作り方や役職のソースコード,REDAMEを参考にさせていただいております。**ykundesu**さんありがとうございます！
- [TownOfHost](https://github.com/tukasa0001/TownOfHost) ※modの作り方や役職のソースコードを参考にさせていただいております。**tukasa0001**さんありがとうございます！
- [ExtremeRoles](https://github.com/yukieiji/ExtremeRoles) ※modの作り方や役職のソースコードを参考にさせていただいております。**yukieiji**さんありがとうございます！


# 開発者
- [しゅれ](https://github.com/oshurecat) ([Twitter](https://twitter.com/shure_NIMDev))/([YouTube](https://www.youtube.com/channel/UCvMjW7DUM0b_TA5TRjJ3BMw))
- [しらたま](https://github.com/Siratamadesu)([Twitter](https://twitter.com/siratamadegesu?s=21&t=s18Ioa3PJ7l4eeZ5WGar-A))/([YouTube](https://youtube.com/channel/UCBGhL8rBMCsbA5Ml6kW_l8A))
- [ハロン](https://github.com/Haroweeeeen)(Twitter)/([YouTube](https://www.youtube.com/channel/UC_ZjRDHgDybTopdkeV7RgVA))
- [シャンパン](https://github.com/Shanpan2)([Twitter](https://twitter.com/shanpanus?s=21&t=VkDFSOnM3bkZQ7Rdw1vNHA))/(YouTube)
- [山菜](https://github.com/sansai0707)([Twitter](https://twitter.com/sansai_yukkuri?s=20&t=VAw9dvpLh1FEFnnUR1W7rA))/([YouTube](https://www.youtube.com/channel/UCj1SxnfqEKlnwXkhCG_VZ7w))

# 開発協力者
- [DekoKiyo](https://github.com/Dekokiyo)さん※脱リアクターなど
- discord参加の皆様

# 内容
## コマンド
| コマンド | 内容 | 使用可能時間 |
| ------ | ---- | ---------- |
| `左Cntl` + `左Shift` | 廃村(ゲームを強制終了させます) | ゲーム開始中 |
| `F11` | 解像度変更<br>480x270 → 640x360 → 800x450 → 1280x720 → 1600x900 → 1920x1080 | いつでも |

## その他設定/機能(ゲーム内)
| 設定項目 | 設定内容 |
| ------- | -------- |
| PC以外をキックする(SNRより) | その名の通りPC以外の端末の人が部屋に入ろうとすると自動的にキックされます。 |
| welcomeメッセージ(TOH/SNRより) | 入った瞬間にここがNIMであるとわかるようにメッセージを送信します。 |

## その他設定/機能(ゲーム外)
| 設定項目 | 設定内容 |
| ------- | -------- |
| NIMのみ部屋検索(SNRより) | このボタンを押すとNIMの部屋しか検索にかからなくなります。 | 

## 役職
| [クルーメイト](#クルーメイト)          | [インポスター](#インポスター)                              | [ニュートラル](#ニュートラル)                   | [両陣営可能](#両陣営可能) |
| --------------------- | ----------------------------------------- | ------------------------- | ---------- |
| [シェリフ](#シェリフ) | [ヴァンパイア](#ヴァンパイア)             | [ジャッカル](#ジャッカル) |  |
| [スニッチ](#スニッチ) | [バウンティハンター](#バウンティハンター) | [妖狐](#妖狐)             |  |
| [ベスト冤罪ヤー](#ベスト冤罪ヤー) | [マッドメイト](#マッドメイト)             |  |  |

## クルーメイト
インポスターと一部のニュートラルを追放すれば勝利することが出来ます。
### シェリフ
開発者：<br>
参考：[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)<br>
陣営：クルーメイト<br>

インポスターなどのクルーメイト陣営以外をくるすることができます。逆にクルーをきるしようとすると自分が死んでしまいます。

設定：
| 設定内容                   | 設定時間感覚                | デフォルト |
| -------------------------- | --------------------------- | ---------- |
| シェリフのキルクールタイム | 10.0秒 ~ 60.0秒 (2.5秒刻み) | 30.0秒     |
| シェリフのキル可能弾数     | 1発 ~ 15発 (1発刻み)        | 3発　      |
| マッドメイトをキルできる   | `ON` / `OFF`                | `OFF`      |
| 第三陣営をキルできる       | `ON` / `OFF` / `個別に設定` | `OFF`      |
| ┣ [ジャッカル](#ジャッカル)をキルできる  | `ON` / `OFF`                | `OFF`      |
| ┣ [妖狐](#妖狐)をキルできる        | `ON` / `OFF`                | `OFF`      |

### スニッチ
開発者：<br>
参考：[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)<br>
陣営：クルーメイト<br>

### ベスト冤罪ヤー
開発者：しゅれ<br>
参考：[SuperNewRoles](https://github.com/ykundesu/SuperNewRoles)<br>
陣営：クルーメイト<br>

最初の会議が終わった時に死んでしまいます。実は役職の作り方がわからなくて試験運用でNIMに導入された役職です。

設定：なし

## インポスター 
クルーメイトとニュートラルを皆殺しにすれば勝利することが出来ます。
### ヴァンパイア
開発者：<br>
参考：[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)<br>
陣営：インポスター<br>

### バウンティハンター
開発者：<br>
参考：[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)<br>
陣営：インポスター<br>

### マッドメイト
開発者：<br>
参考：[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)<br>
陣営：インポスター<br>

## ニュートラル
インポスターとクルーメイト以外にあるもう一つの陣営です。それぞれの勝利条件を満たすため己のために行動します。<br>
別名第三陣営です
### ジャッカル
開発者：<br>
参考：[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)<br>
陣営：ニュートラル(ジャッカル)<br>

### 妖狐
開発者：<br>
参考：[TheOtherRoles](https://github.com/TheOtherRolesAU/TheOtherRoles)<br>
陣営：ニュートラル(妖狐)<br>

## 両陣営可能
Coming soon...

## モード
Coming soon...
