# 今日の予定簡単確認アプリ
de:code 2016 で利用したサンプルアプリのソースコードです。
ソースはサンプルのため極力コメントをつけていますが、不明点や問題があれば Issue に書き込んでください。

Dynamics CRM 2016 Web API/Xamarin.Forms/ADAL/HttpClient/SharePoint Online を利用した予定管理アプリです。
Dynamics CRM より本日の予定を取得してきて、詳細および一覧をタブページで表示します。また Dynamics CRM や Office ネイティブアプリケーションと連携します。

### 必要な環境
Visual Studio 2015 Update 2 以上

### 接続情報
Dynamics CRM Online へは 2016 年 6 月中旬まで以下の認証情報で接続が可能です。
ユーザー名: kenakamu@dcd16prd7.onmicrosoft.com
パスワード: Pa$$w0rd
※自分の環境に接続する場合は、別途 SettingsService.cs ファイルにある変数を更新してください。

### 動作確認環境
UWP - Windows 10 build 10586 および Windows 10 Mobile　build 14332
Android - Nexus 7 Android 6 Marshmallow および Sony Xperia Z5 Android 6 Marshmallow
iOS - iPhone 5 iOS 9.3
