#Region "NSMDConst"
'===========================================================================================================
'   ネームスペース：NSMDConst
'   モジュールＩＤ：MDConst
'   モジュール名称：定数モジュール
'   備考  　　　　：
'===========================================================================================================

Namespace NSMDConst

    Public Module MDConst

        '===============================================================================
        '   定数マスタ
        '===============================================================================
        'Public Const CONSTANT_DTL_ADD_KIND As String = "01"                 ' 自宅
        'Public Const CONSTANT_DTL_ADD_KIND As String = "02"                 ' 単身赴任先住所
        'Public Const CONSTANT_DTL_ADD_KIND As String = "03"                 ' 訓練先
        'Public Const CONSTANT_DTL_ADD_KIND_ETC As String = "99"             ' その他
        'Public Const CONSTANT_DTL_APPLY_AREA_TOKYO As String = "01"         ' 東京
        'Public Const CONSTANT_DTL_APPLY_AREA_OOSAKA As String = "02"        ' 大阪
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "01"           ' 中央執行委員会
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "02"           ' 中央委員会
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "03"           ' 組合大会
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "04"           ' 各専門委員会、部会
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "05"           ' 国際会議
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "06"           ' 中執委員の支部の部会（委員会）
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "07"           ' 臨時特別委員会
        'Public Const CONSTANT_DTL_APPLY_CLASSIFY As String = "99"           ' 上記の会議以外
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "01"        ' 中央執行委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "02"        ' 中央委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "03"        ' 本部書記局
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "04"        ' 教宣部（本部書記局）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "05"        ' 総務部
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "06"        ' 財務部
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "07"        ' 渉外部
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "08"        ' 組織部
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "09"        ' 法務部
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "10"        ' 協約部
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "11"        ' 賃金委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "12"        ' 労働環境委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "13"        ' 安全委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "14"        ' 福利事業委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "15"        ' 調査活動委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "16"        ' ＦＥ問題対策委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "17"        ' 訓練検討委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "18"        ' セニョリティー委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "19"        ' シニア委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "20"        ' 支部委員会（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "21"        ' 支部書記局（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "22"        ' 事務部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "23"        ' 教宣部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "24"        ' 職場組織部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "25"        ' 苦情処理部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "26"        ' 文化活動部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "27"        ' ＮＣＡ部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "28"        ' 訓練センター部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "29"        ' ＡＫＸ部（東京支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "30"        ' 支部委員会（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "31"        ' 支部書記局（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "32"        ' 事務部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "33"        ' 教宣部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "34"        ' 職場組織部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "35"        ' 苦情処理部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "36"        ' 文化活動支部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "37"        ' ＮＣＡ部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "38"        ' 訓練センター部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "39"        ' ＡＫＸ部（大阪支部）
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "40"        ' 監査役員
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "41"        ' 投票管理委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "42"        ' 資格審査委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "43"        ' 航空安全推進連絡会議
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "44"        ' 日本乗員組合連絡会議
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "45"        ' 航空労働連絡担当
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "46"        ' ＡＳＡＰ担当
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "47"        ' 代議員
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "48"        ' 団体交渉
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "49"        ' 労使協議会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "50"        ' 地区協議会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "51"        ' ＰＲＥ地区協
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "52"        ' 懲戒委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "53"        ' 懲戒小委員会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "54"        ' 資格審議会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "55"        ' 事故調査会
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "56"        ' その他
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "57"        ' 公的機関の要請
        'Public Const CONSTANT_DTL_APPLY_MEETINGLIST As String = "58"        ' 支部事務折衝
        'Public Const CONSTANT_DTL_APPLY_STRIKE_LIMIT As String = "01"       ' 1
        'Public Const CONSTANT_DTL_APPLY_STRIKE_LIMIT As String = "02"       ' 10
        'Public Const CONSTANT_DTL_APPLY_STRIKE_LIMIT As String = "03"       ' 6
        'Public Const CONSTANT_DTL_APPLY_STRIKE_LIMIT As String = "04"       ' 720
        'Public Const CONSTANT_DTL_APPLY_STRIKE_LIMIT As String = "05"       ' 45
        'Public Const CONSTANT_DTL_APPLY_STRIKE_LIMIT As String = "06"       ' 45
        'Public Const CONSTANT_DTL_APPLY_STRIKE_LIMIT As String = "07"       ' 15
        'Public Const CONSTANT_DTL_AREA_LOCAL_TOKYO As String = "01"         ' 東京
        'Public Const CONSTANT_DTL_AREA_LOCAL_OOSAKA As String = "02"        ' 大阪
        'Public Const CONSTANT_DTL_AREA_LOCAL_ETC As String = "99"           ' その他
        'Public Const CONSTANT_DTL_BELONGING_TOKYO As String = "01"          ' 東京
        'Public Const CONSTANT_DTL_BELONGING_OOSAKA As String = "02"         ' 大阪
        'Public Const CONSTANT_DTL_BELONGING_ETC As String = "03"            ' その他
        'Public Const CONSTANT_DTL_DAILY_PAY_KIND As String = "01"           ' 部／委員会日当
        'Public Const CONSTANT_DTL_DAILY_PAY_KIND As String = "02"           ' 支部委員会（三役）日当
        'Public Const CONSTANT_DTL_DAILY_PAY_KIND As String = "03"           ' 中央執行日当
        'Public Const CONSTANT_DTL_DAILY_PAY_KIND As String = "04"           ' DGM日当
        'Public Const CONSTANT_DTL_DEPOSIT_ITEMS As String = "01"            ' 普通預金
        'Public Const CONSTANT_DTL_DEPOSIT_ITEMS As String = "02"            ' 当座預金
        'Public Const CONSTANT_DTL_DEPOSIT_ITEMS As String = "03"            ' 貯蓄預金
        'Public Const CONSTANT_DTL_FIX_ADDRESS_INFO_TOKYO As String = "01"   ' 東京
        'Public Const CONSTANT_DTL_FIX_ADDRESS_INFO_OOSAKA As String = "02"  ' 大阪
        'Public Const CONSTANT_DTL_FIX_ADDRESS_INFO As String = "03"         ' 支払者所在地１
        'Public Const CONSTANT_DTL_FIX_ADDRESS_INFO As String = "04"         ' 支払者所在地２
        'Public Const CONSTANT_DTL_INTERNATIONAL_KBN As String = "0"         ' 国内
        'Public Const CONSTANT_DTL_INTERNATIONAL_KBN As String = "1"         ' 海外
        'Public Const CONSTANT_DTL_LAW_OFFICE As String = "01"               ' 東京南部法律事務所
        'Public Const CONSTANT_DTL_LAW_OFFICE As String = "02"               ' 渋谷共同事務所
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "01"             ' 死亡
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "02"             ' 使用者の役員又は利益代表者
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "03"             ' 労働協約による非組合員
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "04"             ' 規約１１条１項の脱退者
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "05"             ' 除名処分
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "06"             ' 禁治産者
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "07"             ' 定年
        'Public Const CONSTANT_DTL_LOS_POSITION As String = "08"             ' 本人の自由意志により会社を退職
        'Public Const CONSTANT_DTL_MODEL_B787 As String = "01"               ' B787
        'Public Const CONSTANT_DTL_MODEL_B777 As String = "02"               ' B777
        'Public Const CONSTANT_DTL_MODEL_B767 As String = "03"               ' B767
        'Public Const CONSTANT_DTL_MODEL_A320 As String = "04"               ' A320
        'Public Const CONSTANT_DTL_MODEL_B744 As String = "05"               ' B744
        'Public Const CONSTANT_DTL_MODEL_B747 As String = "06"               ' B747
        'Public Const CONSTANT_DTL_MODEL_TRANING As String = "98"            ' 訓練機
        'Public Const CONSTANT_DTL_MODEL_ETC As String = "99"                ' その他
        'Public Const CONSTANT_DTL_OFFICER_NAME As String = "01"             ' 伊東　信一郎
        'Public Const CONSTANT_DTL_OUTER_FILES_PATH As String = "01"         ' JikannaiOboe.pdf
        'Public Const CONSTANT_DTL_QUALIFICATION As String = "01"            ' 機長
        'Public Const CONSTANT_DTL_QUALIFICATION As String = "02"            ' 副操縦士
        'Public Const CONSTANT_DTL_QUALIFICATION As String = "03"            ' 航空機関士
        'Public Const CONSTANT_DTL_QUALIFICATION As String = "04"            ' 教官機長
        'Public Const CONSTANT_DTL_QUALIFICATION_ETC As String = "99"        ' その他
        'Public Const CONSTANT_DTL_SENJYU_COMMITTEE_ID As String = "01"      ' SENJYU
        'Public Const CONSTANT_DTL_SENJYU_POST_ID As String = "01"           ' SENJYU
        'Public Const CONSTANT_DTL_SEX_MAN As String = "01"                  ' 男
        'Public Const CONSTANT_DTL_SEX_WOMAN As String = "02"                ' 女
        'Public Const CONSTANT_DTL_SOUGI_KIND As String = "01"               ' 争議行為通告
        'Public Const CONSTANT_DTL_SOUGI_KIND As String = "02"               ' 争議細部通告
        'Public Const CONSTANT_DTL_SOUGI_KIND As String = "03"               ' 全面24Hスト解除通知
        'Public Const CONSTANT_DTL_SOUGI_KIND As String = "04"               ' 一部解除通知
        'Public Const CONSTANT_DTL_SOUGI_KIND As String = "05"               ' 全面24Hスト一部解除通知
        'Public Const CONSTANT_DTL_SOUGI_KIND As String = "06"               ' 争議行為終結通知
        'Public Const CONSTANT_DTL_SOUGI_KIND As String = "07"               ' 申し入れ通知
        'Public Const CONSTANT_DTL_STAF_KIND As String = "01"                ' 正組合員
        'Public Const CONSTANT_DTL_STAF_KIND As String = "02"                ' シニア組合員
        'Public Const CONSTANT_DTL_STAF_KIND As String = "03"                ' 永年組合員
        'Public Const CONSTANT_DTL_STAF_KIND As String = "04"                ' 準組合員
        'Public Const CONSTANT_DTL_STAF_KIND As String = "05"                ' 非組合員
        'Public Const CONSTANT_DTL_STAF_KIND As String = "06"                ' 脱退組合員
        ' 電話番号・FAX情報
        Public Const CONSTANT_DTL_TEL_INFO_TOKYO As String = "01"           ' 東京
        Public Const CONSTANT_DTL_TEL_INFO_OOSAKA As String = "02"          ' 大阪
        'Public Const CONSTANT_DTL_UI_CIR_KIND As String = "01"               ' 合同
        'Public Const CONSTANT_DTL_UI_CIR_KIND As String = "02"               ' TV
        'Public Const CONSTANT_DTL_UI_CIR_KIND As String = "03"               ' 任意
        'Public Const CONSTANT_DTL_UI_CIR_STARTYE_2009 As String = "01"      ' 2009
        'Public Const CONSTANT_DTL_UI_CIR_STARTYE_2010 As String = "02"      ' 2010
        'Public Const CONSTANT_DTL_UI_CIR_STARTYE_2011 As String = "03"      ' 2011
        'Public Const CONSTANT_DTL_UI_SHIBU_TOKYO As String = "01"           ' 東京
        'Public Const CONSTANT_DTL_UI_SHIBU_OOSAKA As String = "02"          ' 大阪
        'Public Const CONSTANT_DTL_UI_SHIBU_ETC As String = "03"             ' その他
        'Public Const CONSTANT_DTL_USER_STATUS As String = "01"              ' 加入
        'Public Const CONSTANT_DTL_USER_STATUS As String = "02"              ' 地位喪失
        'Public Const CONSTANT_DTL_USER_STATUS As String = "03"              ' 脱退
        'Public Const CONSTANT_DTL_WORK_PLACE As String = "01"               ' 乗員室
        'Public Const CONSTANT_DTL_WORK_PLACE As String = "02"               ' 訓練センター
        'Public Const CONSTANT_DTL_WORK_PLACE As String = "03"               ' 本部
        'Public Const CONSTANT_DTL_WORK_PLACE_ETC As String = "99"           ' その他
        'Public Const CONSTANT_DTL_WORK_STATE As String = "01"               ' 移動勤務
        'Public Const CONSTANT_DTL_WORK_STATE As String = "02"               ' 単身赴任

        '===============================================================================
        '   コード・ID
        '===============================================================================
        ' 住所種別
        Public Const ADD_KIND_HOME As String = "01"                                     ' 自宅
        Public Const ADD_KIND_ALON As String = "02"                                     ' 単身赴任先住所
        Public Const ADD_KIND_TRAINING As String = "03"                                 ' 訓練先
        Public Const ADD_KIND_ETC As String = "99"                                      ' その他

        ' 種類
        Public Const APPLY_CLASSIFY_CENTRAL_EXECUTIVE As String = "01"                  ' 中央執行委員会
        Public Const APPLY_CLASSIFY_CENTRAL As String = "02"                            ' 中央委員会
        Public Const APPLY_CLASSIFY_GENERAL_MEETING As String = "03"                    ' 組合大会

        ' 時間内会議名一覧の内容
        'Public Const APPLY_MEETINGLIST As String = "01"                ' tbRet.Rows(0).Item(0).ToString()
        'Public Const APPLY_MEETINGLIST As String = "02"                ' 中央委員会
        'Public Const APPLY_MEETINGLIST As String = "03"                ' 本部書記局
        'Public Const APPLY_MEETINGLIST As String = "04"                ' 教宣部（本部書記局）
        'Public Const APPLY_MEETINGLIST As String = "05"                ' 総務部
        'Public Const APPLY_MEETINGLIST As String = "06"                ' 財務部
        'Public Const APPLY_MEETINGLIST As String = "07"                ' 渉外部
        'Public Const APPLY_MEETINGLIST As String = "08"                ' 組織部
        'Public Const APPLY_MEETINGLIST As String = "09"                ' 法務部
        'Public Const APPLY_MEETINGLIST As String = "10"                ' 協約部
        'Public Const APPLY_MEETINGLIST As String = "11"                ' 賃金委員会
        'Public Const APPLY_MEETINGLIST As String = "12"                ' 労働環境委員会
        'Public Const APPLY_MEETINGLIST As String = "13"                ' 安全委員会
        'Public Const APPLY_MEETINGLIST As String = "14"                ' 福利事業委員会
        'Public Const APPLY_MEETINGLIST As String = "15"                ' 調査活動委員会
        'Public Const APPLY_MEETINGLIST As String = "16"                ' ＦＥ問題対策委員会
        'Public Const APPLY_MEETINGLIST As String = "17"                ' 訓練検討委員会
        'Public Const APPLY_MEETINGLIST As String = "18"                ' セニョリティー委員会
        'Public Const APPLY_MEETINGLIST As String = "19"                ' シニア委員会
        'Public Const APPLY_MEETINGLIST As String = "20"                ' 支部委員会（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "21"                ' 支部書記局（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "22"                ' 事務部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "23"                ' 教宣部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "24"                ' 職場組織部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "25"                ' 苦情処理部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "26"                ' 文化活動部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "27"                ' ＮＣＡ部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "28"                ' 訓練センター部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "29"                ' ＡＫＸ部（東京支部）
        'Public Const APPLY_MEETINGLIST As String = "30"                ' 支部委員会（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "31"                ' 支部書記局（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "32"                ' 事務部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "33"                ' 教宣部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "34"                ' 職場組織部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "35"                ' 苦情処理部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "36"                ' 文化活動支部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "37"                ' ＮＣＡ部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "38"                ' 訓練センター部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "39"                ' ＡＫＸ部（大阪支部）
        'Public Const APPLY_MEETINGLIST As String = "40"                ' 監査役員
        'Public Const APPLY_MEETINGLIST As String = "41"                ' 投票管理委員会
        'Public Const APPLY_MEETINGLIST As String = "42"                ' 資格審査委員会
        'Public Const APPLY_MEETINGLIST As String = "43"                ' 航空安全推進連絡会議
        'Public Const APPLY_MEETINGLIST As String = "44"                ' 日本乗員組合連絡会議
        'Public Const APPLY_MEETINGLIST As String = "45"                ' 航空労働連絡担当
        'Public Const APPLY_MEETINGLIST As String = "46"                ' ＡＳＡＰ担当
        'Public Const APPLY_MEETINGLIST As String = "47"                ' 代議員
        'Public Const APPLY_MEETINGLIST As String = "48"                ' 団体交渉
        'Public Const APPLY_MEETINGLIST As String = "49"                ' 労使協議会
        'Public Const APPLY_MEETINGLIST As String = "50"                ' 地区協議会
        'Public Const APPLY_MEETINGLIST As String = "51"                ' ＰＲＥ地区協
        'Public Const APPLY_MEETINGLIST As String = "52"                ' 懲戒委員会
        'Public Const APPLY_MEETINGLIST As String = "53"                ' 懲戒小委員会
        'Public Const APPLY_MEETINGLIST As String = "54"                ' 資格審議会
        'Public Const APPLY_MEETINGLIST As String = "55"                ' 事故調査会
        'Public Const APPLY_MEETINGLIST As String = "56"                ' その他
        'Public Const APPLY_MEETINGLIST As String = "57"                ' 公的機関の要請
        'Public Const APPLY_MEETINGLIST As String = "58"                ' 支部事務折衝

        ' 会社所属区分
        Public Const AREA_LOCAL_TOKYO As String = "01"                                  ' 東京
        Public Const AREA_LOCAL_OSAKA As String = "02"                                  ' 大阪
        Public Const AREA_LOCAL_ETC As String = "99"                                    ' その他

        ' 組合支部区分
        Public Const BELONGING_TOKYO As String = "01"                                   ' 東京
        Public Const BELONGING_OSAKA As String = "02"                                   ' 大阪
        Public Const BELONGING_ETC As String = "03"                                     ' その他

        ' 所属会社
        Public Const ATTACH_COMPANY_ANA As Byte = 1                                     ' ANA
        Public Const ATTACH_COMPANY_NCA As Byte = 2                                     ' NCA
        Public Const ATTACH_COMPANY_AKX As Byte = 3                                     ' AKX

        ' 会社所属
        Public Const COMPANY_ATTACH_TOKYO As Byte = 1                                   ' 東京
        Public Const COMPANY_ATTACH_OOSAKA As Byte = 2                                  ' 大阪

        ' 組合支部
        Public Const UNION_BRANCH_TOKYO As Byte = 1                                     ' 東京
        Public Const UNION_BRANCH_OOSAKA As Byte = 2                                    ' 大阪

        ' 固定所在地情報の内容
        Public Const FIX_ADDRESS_INFO_TOKYO As String = "01"                            ' 東京
        Public Const FIX_ADDRESS_INFO_OSAKA As String = "02"                            ' 大阪
        'Public Const FIX_ADDRESS_INFO As String = "03"                                  ' 支払者所在地１
        'Public Const FIX_ADDRESS_INFO As String = "04"                                  ' 支払者所在地２

        ' 専従職員給与にて仕様
        'Public Const LAW_OFFICE As String = "01"                                        ' 東京南部法律事務所
        'Public Const LAW_OFFICE As String = "02"                                        ' 渋谷共同事務所

        ' 地位喪失理由区分
        'Public Const LOS_POSITION As String = "01"                                      ' 死亡
        'Public Const LOS_POSITION As String = "02"                                      ' 使用者の役員又は利益代表者
        'Public Const LOS_POSITION As String = "03"                                      ' 労働協約による非組合員
        'Public Const LOS_POSITION As String = "04"                                      ' 規約１１条１項の脱退者
        'Public Const LOS_POSITION As String = "05"                                      ' 除名処分
        'Public Const LOS_POSITION As String = "06"                                      ' 禁治産者
        'Public Const LOS_POSITION As String = "07"                                      ' 定年
        'Public Const LOS_POSITION As String = "08"                                      ' 本人の自由意志により会社を退職

        ' 機種
        Public Const MODEL_B787 As String = "01"                                        ' B787
        Public Const MODEL_B777 As String = "02"                                        ' B777
        Public Const MODEL_B767 As String = "03"                                        ' B767
        Public Const MODEL_A320 As String = "04"                                        ' A320
        Public Const MODEL_B744 As String = "05"                                        ' B744
        Public Const MODEL_B747 As String = "06"                                        ' B747
        Public Const MODEL_B737 As String = "07"                                        ' B737
        Public Const MODEL_TRAINING As String = "98"                                    ' 訓練機
        Public Const MODEL_ETC As String = "99"                                         ' その他

        ' 社長名
        Public Const OFFICER_NAME As String = "伊東　信一郎"                            ' 社長名

        ' 乗務資格
        Public Const QUALIFICATION_PILOT As String = "01"                               ' 機長
        Public Const QUALIFICATION_COPILOT As String = "02"                             ' 副操縦士
        Public Const QUALIFICATION_FLIGHT_ENGINEER As String = "03"                     ' 航空機関士
        Public Const QUALIFICATION_TEACHER_PILOT As String = "04"                       ' 教官機長
        Public Const QUALIFICATION_ETC As String = "99"                                 ' その他

        ' 電話番号情報の内容
        Public Const TEL_INFO_TOKYO As String = "01"                                    ' 東京
        Public Const TEL_INFO_OSAKA As String = "02"                                    ' 大阪

        ' 組合大会通知 開催開始年
        Public Const UI_CIR_STARTYE_2009 As String = "01"                               ' 2009
        Public Const UI_CIR_STARTYE_2010 As String = "02"                               ' 2010
        Public Const UI_CIR_STARTYE_2011 As String = "03"                               ' 2011

        ' 組合大会通知 種別
        Public Const UI_CIR_KIND_JOIN As String = "01"                                  ' 合同
        Public Const UI_CIR_KIND_TV As String = "02"                                    ' TV
        Public Const UI_CIR_KIND_ANY As String = "03"                                   ' 任意

        ' 支部
        Public Const UI_SHIBU_TOKYO As String = "01"                                    ' 東京
        Public Const UI_SHIBU_OSAKA As String = "02"                                    ' 大阪
        Public Const UI_SHIBU_ETC As String = "03"                                      ' その他

        ' (スケジュール用)支部
        Public Const SCD_SHIBU_JOIN As String = "01"                                    ' 合同
        Public Const SCD_SHIBU_TOKYO As String = "02"                                   ' 東京
        Public Const SCD_SHIBU_OSAKA As String = "03"                                   ' 大阪

        ' 職場
        Public Const WORK_PLACE_CREW As String = "01"                                   ' 乗務室
        Public Const WORK_PLACE_TRAINING As String = "02"                               ' 訓練センター
        Public Const WORK_PLACE_HEADQUARTERS As String = "03"                           ' 本部
        Public Const WORK_PLACE_ETC As String = "99"                                    ' その他

        ' 勤務形態
        Public Const WORK_STATE_ALONE As String = "01"                                  ' 単身赴任
        Public Const WORK_STATE_MOVEWORK As String = "02"                               ' 移動勤務

        ' 国内海外区分
        Public Const INTERNATIONAL_KBN_HOME As String = "0"                             ' 国内
        Public Const INTERNATIONAL_KBN_ABROAD As String = "1"                           ' 海外

        ' 組合員ステータス区分
        Public Const USER_STATUS_ENTRY As String = "01"                                 ' 加入
        Public Const USER_STATUS_POSITION_LOSS As String = "02"                         ' 地位喪失
        Public Const USER_STATUS_LEAVE As String = "03"                                 ' 脱退

        ' 組合員種別コード
        Public Const STAF_KIND_REGULAR As String = "01"                                 ' 正組合員
        Public Const STAF_KIND_SENIOR As String = "02"                                  ' シニア組合員
        Public Const STAF_KIND_LONGTIME As String = "03"                                ' 永年組合員
        Public Const STAF_KIND_SEMI As String = "04"                                    ' 準組合員
        Public Const STAF_KIND_NON As String = "05"                                     ' 非組合員

        ' 性別
        Public Const SEX_MAN As String = "01"                                           ' 男
        Public Const SEX_WOMAN As String = "02"                                         ' 女

        ' メイン画面
        Public Const MAIN_PANEL_ID As String = "pnlMain"                                ' メイン操作パネルID

        ' コンボボックス　ドロップダウンリストスタイル
        Public Const COMBO_STYLE_DROPDOWNLIST_SIMPLE As Byte = 0                        ' テキスト編集可能
        Public Const COMBO_STYLE_DROPDOWNLIST_DROPDOWN As Byte = 1                      ' テキスト編集可能
        Public Const COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST As Byte = 2                  ' テキスト編集不可

        ' 帳票出力済みフラグ
        Public Const K_DOCUMENT_OUT_NONE As Byte = 0                                    ' 未出力
        Public Const K_DOCUMENT_OUT_ADD_DELETE As Byte = 1                              ' 追加削除
        Public Const K_DOCUMENT_OUT_CHANGE As Byte = 2                                  ' 変更

        ' 委員会更新情報 追加対象フラグ
        Public Const K_COMMITTEE_INSERT_ADD As String = "0"                             ' 追加
        Public Const K_COMMITTEE_INSERT_DELETE As String = "1"                          ' 削除
        Public Const K_COMMITTEE_INSERT_CHANGE As String = "2"                          ' 変更

        '===============================================================================
        '   メニューID
        '===============================================================================
        Public Const MENU_ID_UC010101 As String = "UC010101"                            ' 組合員管理
        Public Const MENU_ID_UC020101 As String = "UC020101"                            ' 部/委員会名簿
        Public Const MENU_ID_UC020201 As String = "UC020201"                            ' 日程表
        Public Const MENU_ID_UC020301 As String = "UC020301"                            ' 会議通知
        Public Const MENU_ID_UC020401 As String = "UC020401"                            ' 出欠簿
        Public Const MENU_ID_UC020501 As String = "UC020501"                            ' 定型名簿
        Public Const MENU_ID_UC020601 As String = "UC020601"                            ' 検索
        Public Const MENU_ID_UC030101 As String = "UC030101"                            ' 所属委員会
        Public Const MENU_ID_UC030201 As String = "UC030201"                            ' 期別名簿検索
        Public Const MENU_ID_UC030301 As String = "UC030301"                            ' 中執活動報告
        Public Const MENU_ID_UC030401 As String = "UC030401"                            ' 委員データ出力
        Public Const MENU_ID_UC040101 As String = "UC040101"                            ' 組合大会通知
        Public Const MENU_ID_UC040201 As String = "UC040201"                            ' 争議行為
        Public Const MENU_ID_UC040301 As String = "UC040301"                            ' 時間内組合活動
        Public Const MENU_ID_UC040401 As String = "UC040401"                            ' 指名ストライキ
        Public Const MENU_ID_UC040601 As String = "UC040601"                            ' 発信文書
        Public Const MENU_ID_UC040701 As String = "UC040701"                            ' 住所変更出力
        Public Const MENU_ID_UC040801 As String = "UC040801"                            ' UP・UJデータ出力
        Public Const MENU_ID_UC050101 As String = "UC050101"                            ' 賃金カット
        Public Const MENU_ID_UC050201 As String = "UC050201"                            ' 日当計算
        Public Const MENU_ID_UC050301 As String = "UC050301"                            ' 源泉徴収
        Public Const MENU_ID_UC050401 As String = "UC050401"                            ' 収支予想
        Public Const MENU_ID_UC050501 As String = "UC050501"                            ' 銀行口座一覧出力
        Public Const MENU_ID_UC050601 As String = "UC050601"                            ' 日当データ出力
        Public Const MENU_ID_UC060101 As String = "UC060101"                            ' 選挙人名簿
        Public Const MENU_ID_UC080101 As String = "UC080101"                            ' 労金データ作成
        Public Const MENU_ID_UC080201 As String = "UC080201"                            ' 組合員口座情報
        Public Const MENU_ID_FM090101 As String = "FM090101"                            ' マスタメンテナンス
        Public Const MENU_ID_FM090201 As String = "FM090201"                            ' 財務管理マスタメンテナンス
        Public Const MENU_ID_FM090301 As String = "FM090301"                            ' 管理部登録
        Public Const MENU_ID_FM090401 As String = "FM090401"                            ' 発信文書マスタメンテナンス

        '===============================================================================
        '   メニュー名
        '===============================================================================
        Public Const MENU_NAME_UC010101 As String = "組合員管理"
        Public Const MENU_NAME_UC020101 As String = "部/委員会名簿"
        Public Const MENU_NAME_UC020201 As String = "日程表"
        Public Const MENU_NAME_UC020301 As String = "会議通知"
        Public Const MENU_NAME_UC020401 As String = "出欠簿"
        Public Const MENU_NAME_UC020501 As String = "定型名簿"
        Public Const MENU_NAME_UC020601 As String = "検索"
        Public Const MENU_NAME_UC030101 As String = "所属委員会"
        Public Const MENU_NAME_UC030201 As String = "期別名簿検索"
        Public Const MENU_NAME_UC030301 As String = "中執活動報告"
        Public Const MENU_NAME_UC030401 As String = "委員会データ出力"
        Public Const MENU_NAME_UC040101 As String = "組合大会通知"
        Public Const MENU_NAME_UC040201 As String = "争議行為"
        Public Const MENU_NAME_UC040301 As String = "時間内組合活動"
        Public Const MENU_NAME_UC040401 As String = "指名ストライキ"
        Public Const MENU_NAME_UC040601 As String = "発信文書"
        Public Const MENU_NAME_UC040701 As String = "住所変更出力"
        Public Const MENU_NAME_UC040801 As String = "UP・UJデータ出力"
        Public Const MENU_NAME_UC050101 As String = "賃金カット"
        Public Const MENU_NAME_UC050201 As String = "日当計算"
        Public Const MENU_NAME_UC050301 As String = "源泉徴収"
        Public Const MENU_NAME_UC050401 As String = "収支予想"
        Public Const MENU_NAME_UC050501 As String = "銀行口座一覧"
        Public Const MENU_NAME_UC050601 As String = "日当データ出力"
        Public Const MENU_NAME_UC060101 As String = "選挙人名簿"
        Public Const MENU_NAME_UC080101 As String = "労金データ作成"
        Public Const MENU_NAME_UC080201 As String = "組合員口座情報"
        Public Const MENU_NAME_FM090101 As String = "マスタメンテナンス"
        Public Const MENU_NAME_FM090201 As String = "財務管理マスタメンテナンス"
        Public Const MENU_NAME_FM090301 As String = "管理部登録"
        Public Const MENU_NAME_FM090401 As String = "発信文書マスタメンテナンス"

        '===============================================================================
        '   画面ID
        '===============================================================================
        Public Const SCREEN_ID_FM000101 As String = "FM000101"                          ' ユーザ認証画面
        Public Const SCREEN_ID_FM000102 As String = "FM000102"                          ' メイン操作画面
        Public Const SCREEN_ID_FM000103 As String = "FM000103"                          ' パスワード変更画面
        Public Const SCREEN_ID_FM000104 As String = "FM000104"                          ' 部／委員会選択画面
        Public Const SCREEN_ID_FM000105 As String = "FM000105"                          ' エラーメッセージ画面

        'Public Const SCREEN_ID_FM000201 As String = "FM000201"                          ' 適用日付選択画面
        Public Const SCREEN_ID_FM000202 As String = "FM000202"                          ' 住所検索結果画面
        Public Const SCREEN_ID_FM000203 As String = "FM000203"                          ' 印刷プレビュー画面
        Public Const SCREEN_ID_FM000204 As String = "FM000204"                          ' 配布者選択画面
        Public Const SCREEN_ID_FM000205 As String = "FM000205"                          ' 印刷プレビュー画面
        Public Const SCREEN_ID_FM000206 As String = "FM000206"                          ' 委員会名簿履歴一覧画面
        Public Const SCREEN_ID_FM000207 As String = "FM000207"                          ' データベースメンテナンス画面

        Public Const SCREEN_ID_UC010101 As String = "UC010101"                          ' 組合員検索画面
        Public Const SCREEN_ID_UC010102 As String = "UC010102"                          ' 組合員管理 - 基本情報画面
        Public Const SCREEN_ID_UC010103 As String = "UC010103"                          ' 組合員管理 - 住所情報画面
        Public Const SCREEN_ID_FM010104 As String = "FM010104"                          ' 基本情報履歴選択画面
        Public Const SCREEN_ID_FM010105 As String = "FM010105"                          ' 基準日入力画面

        Public Const SCREEN_ID_UC010201 As String = "UC010201"                          ' 固定フォーマット出力画面

        Public Const SCREEN_ID_UC020101 As String = "UC020101"                          ' 部／委員会名簿画面
        Public Const SCREEN_ID_FM020102 As String = "FM020102"                          ' 委員会名簿履歴一覧画面
        Public Const SCREEN_ID_FM020103 As String = "FM020103"                          ' 印刷項目選択画面

        Public Const SCREEN_ID_UC020201 As String = "UC020201"                          ' 日程表画面
        Public Const SCREEN_ID_UC020202 As String = "UC020202"                          ' 日程表詳細画面
        Public Const SCREEN_ID_UC020203 As String = "UC020203"                          ' 日程表詳細 - 新規登録画面

        Public Const SCREEN_ID_UC020301 As String = "UC020301"                          ' 会議通知画面
        Public Const SCREEN_ID_UC020302 As String = "UC020302"                          ' 会議通知 - 新規登録画面画面
        Public Const SCREEN_ID_UC020303 As String = "UC020303"                          ' 会議通知 - 合同登録画面

        Public Const SCREEN_ID_UC020401 As String = "UC020401"                          ' 出欠簿画面
        Public Const SCREEN_ID_FM020402 As String = "FM020402"                          ' 出欠簿登録状況一覧画面

        Public Const SCREEN_ID_UC020501 As String = "UC020501"                          ' 定型名簿画面

        Public Const SCREEN_ID_UC020601 As String = "UC020601"                          ' 検索画面
        Public Const SCREEN_ID_FM020602 As String = "FM020602"                          ' 検索結果画面

        Public Const SCREEN_ID_UC030101 As String = "UC030101"                          ' 所属委員会画面

        Public Const SCREEN_ID_UC030201 As String = "UC030201"                          ' 期別名簿検索画面
        'Public Const SCREEN_ID_FM030202 As String = "FM030202"                          ' 委員会名簿履歴一覧画面

        Public Const SCREEN_ID_UC030301 As String = "UC030301"                          ' 中執活動報告画面
        Public Const SCREEN_ID_FM030302 As String = "FM030302"                          ' 登録状況一覧画面

        Public Const SCREEN_ID_UC030401 As String = "UC030401"                          ' 委員会データ出力画面

        Public Const SCREEN_ID_UC040101 As String = "UC040101"                          ' 組合大会通知画面
        Public Const SCREEN_ID_UC040102 As String = "UC040102"                          ' 組合大会通知 - 新規登録画面
        Public Const SCREEN_ID_FM040103 As String = "FM040103"                          ' 開催登録　種別・支部選択画面
        Public Const SCREEN_ID_UC040104 As String = "UC040104"                          ' 組合大会通知 - 詳細画面
        Public Const SCREEN_ID_FM040104 As String = "FM040104"                          ' 開催登録　種別・支部選択画面

        Public Const SCREEN_ID_UC040201 As String = "UC040201"                          ' 争議行為画面
        Public Const SCREEN_ID_UC040202 As String = "UC040202"                          ' 争議行為通告 - 詳細画面
        Public Const SCREEN_ID_UC040203 As String = "UC040203"                          ' 労働協約第47条申し入れ - 詳細画面
        Public Const SCREEN_ID_UC040204 As String = "UC040204"                          ' 争議行為細部通告 - 詳細画面
        Public Const SCREEN_ID_UC040205 As String = "UC040205"                          ' 争議行為解除通知 - 詳細画面
        Public Const SCREEN_ID_UC040206 As String = "UC040206"                          ' 争議行為終結通知 - 詳細画面
        Public Const SCREEN_ID_FM040207 As String = "FM040207"                          ' 別紙情報登録画面画面

        Public Const SCREEN_ID_UC040301 As String = "UC040301"                          ' 時間内組合活動画面
        Public Const SCREEN_ID_UC040302 As String = "UC040302"                          ' 時間内組合活動 - 申請画面
        Public Const SCREEN_ID_FM040303 As String = "FM040303"                          ' 種類の選択画面
        Public Const SCREEN_ID_FM040304 As String = "FM040304"                          ' 中央員会の日程新規追加画面
        Public Const SCREEN_ID_FM040305 As String = "FM040305"                          ' 時間内組合活動申請可能残数一覧画面
        Public Const SCREEN_ID_UC040306 As String = "UC040306"                          ' 時間内組合活動 - 取消画面画面

        Public Const SCREEN_ID_UC040401 As String = "UC040401"                          ' 指名ストライキ画面
        Public Const SCREEN_ID_UC040402 As String = "UC040402"                          ' 指名ストライキ - 通告画面
        Public Const SCREEN_ID_UC040403 As String = "UC040403"                          ' 指名ストライキ - 一部解除画面
        Public Const SCREEN_ID_FM040404 As String = "FM040404"                          ' 争議行為通告番号選択画面
        Public Const SCREEN_ID_UC040405 As String = "UC040405"                          ' 氏名ストライキ - 通告画面

        Public Const SCREEN_ID_UC040501 As String = "UC040501"                          ' 当直スケジュール画面
        Public Const SCREEN_ID_UC040502 As String = "UC040502"                          ' 当直スケジュール - 詳細画面
        Public Const SCREEN_ID_FM040503 As String = "FM040503"                          ' 印刷対象日選択画面

        Public Const SCREEN_ID_UC040601 As String = "UC040601"                          ' 発信文書画面
        Public Const SCREEN_ID_FM040602 As String = "FM040602"                          ' 発信文書新規作成
        Public Const SCREEN_ID_FM040603 As String = "FM040603"                          ' 発信文書ファイル名入力
        Public Const SCREEN_ID_FM040604 As String = "FM040604"                          ' 文書操作ウィンドウ画面
        Public Const SCREEN_ID_FM040605 As String = "FM040605"                          ' コピー編集期選択画面

        Public Const SCREEN_ID_UC040701 As String = "UC040701"                          ' 住所変更出力画面

        Public Const SCREEN_ID_UC040801 As String = "UC040801"                          ' UP・UJデータ出力画面

        Public Const SCREEN_ID_UC050101 As String = "UC050101"                          ' 賃金カット画面
        Public Const SCREEN_ID_UC050102 As String = "UC050102"                          ' 賃金カット - 月例・時間内画面
        Public Const SCREEN_ID_UC050103 As String = "UC050103"                          ' 賃金カット - 一時金画面

        Public Const SCREEN_ID_UC050201 As String = "UC050201"                          ' 日当計算画面
        Public Const SCREEN_ID_UC050202 As String = "UC050102"                          ' 委員日当計算 - 詳細画面
        Public Const SCREEN_ID_UC050203 As String = "UC050203"                          ' 中執日当計算 - 詳細画面

        Public Const SCREEN_ID_UC050301 As String = "UC050301"                          ' 源泉徴収画面
        Public Const SCREEN_ID_UC050302 As String = "UC050302"                          ' 源泉徴収 － 課税対象者画面
        Public Const SCREEN_ID_FM050303 As String = "FM050303"                          ' 集計対象年月の選択画面
        Public Const SCREEN_ID_UC050304 As String = "UC050304"                          ' 源泉徴収 － 課税対象者一時金画面

        Public Const SCREEN_ID_UC050401 As String = "UC050401"                          ' 収支予想画面
        Public Const SCREEN_ID_UC050402 As String = "UC050402"                          ' 収支予想 - 新規登録画面
        Public Const SCREEN_ID_UC050403 As String = "UC050403"                          ' 乗員計画状況 - 新規登録画面
        Public Const SCREEN_ID_FM050404 As String = "FM050404"                          ' 乗員計画新規登録画面
        Public Const SCREEN_ID_UC050405 As String = "UC050405"                          ' 分担金状況 - 新規登録画面
        Public Const SCREEN_ID_UC050406 As String = "UC050406"                          ' 予算登録 - 新規登録画面
        Public Const SCREEN_ID_UC050407 As String = "UC050407"                          ' 修正予算 － 新規登録画面

        Public Const SCREEN_ID_UC050501 As String = "UC050501"                          ' 銀行口座一覧出力画面

        Public Const SCREEN_ID_UC050601 As String = "UC050601"                          ' 日当データ出力画面

        Public Const SCREEN_ID_UC060101 As String = "UC060101"                          ' 選挙人名簿画面

        Public Const SCREEN_ID_UC080101 As String = "UC080101"                          ' 労金データ作成画面
        Public Const SCREEN_ID_FM080102 As String = "FM080102"                          ' 振込データ未作成の組合員画面
        Public Const SCREEN_ID_FM080103 As String = "FM080103"                          ' 振込データ新規作成画面
        Public Const SCREEN_ID_UC080104 As String = "UC080104"                          ' 労金データ作成 － 新規登録画面

        Public Const SCREEN_ID_UC080201 As String = "UC080201"                          ' 組合員口座情報画面
        Public Const SCREEN_ID_UC080202 As String = "UC080202"                          ' 組合員口座情報 - 詳細画面

        Public Const SCREEN_ID_FM090101 As String = "FM090101"                          ' マスタメンテナンス画面
        Public Const SCREEN_ID_UC090102 As String = "UC090102"                          ' 定数マスタメンテナンス画面
        Public Const SCREEN_ID_UC090104 As String = "UC090104"                          ' パスワードマスタメンテナンス画面
        Public Const SCREEN_ID_UC090105 As String = "UC090105"                          ' パスワードマスタメンテナンス - パスワードリセット画面
        Public Const SCREEN_ID_UC090106 As String = "UC090106"                          ' 委員会マスタメンテナンス画面
        Public Const SCREEN_ID_UC090107 As String = "UC090107"                          ' 委員会マスタメンテナンス - 新規登録画面
        Public Const SCREEN_ID_UC090110 As String = "UC090110"                          ' 期マスタメンテナンス画面
        Public Const SCREEN_ID_UC090112 As String = "UC090112"                          ' 日当マスタメンテナンス画面
        Public Const SCREEN_ID_UC090113 As String = "UC090113"                          ' 日当マスタメンテナンス画面 - 新規登録画面
        Public Const SCREEN_ID_UC090114 As String = "UC090114"                          ' 中央執行昼食費マスタメンテナンス
        Public Const SCREEN_ID_UC090115 As String = "UC090115"                          ' 中央執行昼食費マスタメンテナンス - 新規登録画面
        Public Const SCREEN_ID_UC090116 As String = "UC090116"                          ' 役員手当マスタメンテナンス画面
        Public Const SCREEN_ID_UC090117 As String = "UC090117"                          ' 役員手当マスタメンテナンス - 新規登録画面
        Public Const SCREEN_ID_UC090118 As String = "UC090118"                          ' 課税率マスタメンテナンス画面
        Public Const SCREEN_ID_UC090120 As String = "UC090120"                          ' 切捨て金額マスタメンテナンス画面
        Public Const SCREEN_ID_UC090122 As String = "UC090122"                          ' 郵便番号マスタメンテナンス画面
        Public Const SCREEN_ID_FM090122 As String = "FM090122"                          ' 郵便番号マスタ更新中画面
        Public Const SCREEN_ID_UC090123 As String = "UC090123"                          ' 画面マスタメンテナンス画面
        Public Const SCREEN_ID_UC090125 As String = "UC090125"                          ' メニューコントロールマスタメンテナンス画面
        Public Const SCREEN_ID_UC090128 As String = "UC090128"                          ' 銀行マスタメンテナンス画面
        Public Const SCREEN_ID_UC090130 As String = "UC090130"                          ' 専従職員権限マスタメンテナンス画面
        Public Const SCREEN_ID_UC090131 As String = "UC090131"                          ' 専従職員権限マスタメンテナンス画面 - 新規登録画面
        Public Const SCREEN_ID_FM090131 As String = "FM090131"                          ' 専従職員権限マスタメンテナンス画面 - 権限詳細画面
        Public Const SCREEN_ID_UC090133 As String = "UC090133"                          ' 会社マスタメンテナンス画面
        Public Const SCREEN_ID_UC090137 As String = "UC090137"                          ' 権限マスタメンテナンス画面

        Public Const SCREEN_ID_FM090201 As String = "FM090201"                          ' 財務管理マスタメンテナンス画面
        Public Const SCREEN_ID_UC090202 As String = "UC090202"                          ' 支出科目マスタメンテナンス画面
        Public Const SCREEN_ID_UC090203 As String = "UC090203"                          ' 支出科目マスタメンテナンス画面
        Public Const SCREEN_ID_UC090204 As String = "UC090204"                          ' 組合費マスタメンテナンス画面
        Public Const SCREEN_ID_UC090205 As String = "UC090205"                          ' 組合費マスタメンテナンス - 新規登録画面

        Public Const SCREEN_ID_FM090301 As String = "FM090301"                          ' 管理部登録画面
        Public Const SCREEN_ID_UC090302 As String = "UC090302"                          ' 管理部名簿登録画面
        Public Const SCREEN_ID_UC090303 As String = "UC090303"                          ' 管理部マスタメンテナンス画面
        Public Const SCREEN_ID_UC090304 As String = "UC090304"                          ' 管理部マスタメンテナンス - 新規登録画面
        Public Const SCREEN_ID_FM090305 As String = "FM090305"                          ' 画面操作権限画面
        Public Const SCREEN_ID_FM090306 As String = "FM090306"                          ' 画面操作権限詳細画面
        Public Const SCREEN_ID_UC090307 As String = "UC090307"                          ' 管理部名簿登録画面

        Public Const SCREEN_ID_FM090401 As String = "FM090401"                          ' 発信文書マスタメンテナンス画面
        Public Const SCREEN_ID_UC090402 As String = "UC090402"                          ' 発信文書宛先マスタメンテナンス画面
        Public Const SCREEN_ID_UC090403 As String = "UC090403"                          ' 発信文書宛先マスタメンテナンス - 新規登録画面
        Public Const SCREEN_ID_UC090404 As String = "UC090404"                          ' 発信文書差出マスタメンテナンス画面
        Public Const SCREEN_ID_UC090405 As String = "UC090405"                          ' 発信文書差出マスタメンテナンス - 新規登録画面
        Public Const SCREEN_ID_FM090406 As String = "FM090406"                          ' 部／委員会・役職選択画面画面
        Public Const SCREEN_ID_UC090407 As String = "UC090407"                          ' 発信文書委員会マスタメンテナンス画面
        Public Const SCREEN_ID_UC090408 As String = "UC090408"                          ' 発信文書委員会マスタメンテナンス - 新規登録画面
        Public Const SCREEN_ID_UC090409 As String = "UC090409"                          ' 委員会情報設定画面
        Public Const SCREEN_ID_UC090410 As String = "UC090410"                          ' 都道府県空港マスタメンテナンス画面
        Public Const SCREEN_ID_UC090411 As String = "UC090411"                          ' 都道府県空港マスタメンテナンス - 新規登録画面

        Public Const SCREEN_ID_UC100101 As String = "UC100101"                          ' DGM管理画面

        Public Const SCREEN_ID_UC999999 As String = "UC999999"                          ' 複数エラーメッセージ表示画面
        Public Const SCREEN_ID_UCInfoMsg As String = "UCInfoMsg"                        ' 複数注意メッセージ表示画面

        Public Const SCREEN_ID_CLACCESSMDB As String = "CLAccessMdb"                    ' データベース関連クラス
        Public Const SCREEN_ID_CLENCRYPT As String = "CLEncrypt"                        ' 暗号化復号化関連クラス
        Public Const SCREEN_ID_CLMSG As String = "CLMsg"                                ' メッセージ関連クラス
        Public Const SCREEN_ID_MDCHK As String = "MDChk"                                ' チェック関連モジュール
        Public Const SCREEN_ID_MDCOMMON As String = "MDCommon"                          ' 共通モジュール
        Public Const SCREEN_ID_MDCONST As String = "MDConst"                            ' 定数モジュール
        Public Const SCREEN_ID_MDFILE As String = "MDFile"                              ' ファイル関連モジュール
        Public Const SCREEN_ID_MDINFO As String = "MDInfo"                              ' 情報関連モジュール

        '===============================================================================
        '   画面名
        '===============================================================================
        Public Const SCREEN_NAME_FM000101 As String = "ユーザ認証画面"
        Public Const SCREEN_NAME_FM000102 As String = "メイン操作画面"
        Public Const SCREEN_NAME_FM000103 As String = "パスワード変更画面"
        Public Const SCREEN_NAME_FM000104 As String = "部／委員会選択画面"
        Public Const SCREEN_NAME_FM000105 As String = "エラーメッセージ画面"

        'Public Const SCREEN_NAME_000201 As String = "適用日付選択画面"
        Public Const SCREEN_NAME_FM000202 As String = "住所検索結果画面"
        Public Const SCREEN_NAME_FM000203 As String = "印刷プレビュー画面"
        Public Const SCREEN_NAME_FM000204 As String = "配布者選択画面"
        Public Const SCREEN_NAME_FM000205 As String = "印刷プレビュー画面"
        Public Const SCREEN_NAME_FM000206 As String = "委員会名簿履歴一覧画面"
        Public Const SCREEN_NAME_FM000207 As String = "データベースメンテナンス画面"

        Public Const SCREEN_NAME_UC010101 As String = "組合員検索画面"
        Public Const SCREEN_NAME_UC010102 As String = "組合員管理 - 基本情報画面"
        Public Const SCREEN_NAME_UC010103 As String = "組合員管理 - 住所情報画面"
        Public Const SCREEN_NAME_FM010104 As String = "適用日付選択画面"
        'Public Const SCREEN_NAME_FM010104 As String = "基本情報履歴選択画面"
        Public Const SCREEN_NAME_FM010105 As String = "基準日入力画面"

        Public Const SCREEN_NAME_UC010201 As String = "固定フォーマット出力画面"

        Public Const SCREEN_NAME_UC020101 As String = "部／委員会名簿画面"
        Public Const SCREEN_NAME_FM020102 As String = "委員会名簿履歴一覧画面"
        Public Const SCREEN_NAME_FM020103 As String = "印刷項目選択画面"

        Public Const SCREEN_NAME_UC020201 As String = "日程表画面"
        Public Const SCREEN_NAME_UC020202 As String = "日程表詳細画面"
        Public Const SCREEN_NAME_UC020203 As String = "日程表詳細 - 新規登録画面"

        Public Const SCREEN_NAME_UC020301 As String = "会議通知画面"
        Public Const SCREEN_NAME_UC020302 As String = "会議通知 - 新規登録画面画面"
        Public Const SCREEN_NAME_UC020303 As String = "会議通知 - 合同登録画面"

        Public Const SCREEN_NAME_UC020401 As String = "出欠簿画面"
        Public Const SCREEN_NAME_FM020402 As String = "出欠簿登録状況一覧画面"

        Public Const SCREEN_NAME_UC020501 As String = "定型名簿画面"

        Public Const SCREEN_NAME_UC020601 As String = "検索画面"
        Public Const SCREEN_NAME_FM020602 As String = "検索結果画面"

        Public Const SCREEN_NAME_UC030101 As String = "所属委員会画面"

        Public Const SCREEN_NAME_UC030201 As String = "期別名簿検索画面"
        'Public Const SCREEN_NAME_FM030202 As String = "委員会名簿履歴一覧画面"

        Public Const SCREEN_NAME_UC030301 As String = "中執活動報告画面"
        Public Const SCREEN_NAME_FM030302 As String = "登録状況一覧画面"

        Public Const SCREEN_NAME_UC030401 As String = "委員会データ出力画面"

        Public Const SCREEN_NAME_UC040101 As String = "組合大会通知画面"
        Public Const SCREEN_NAME_UC040102 As String = "組合大会通知 - 新規登録画面"
        Public Const SCREEN_NAME_FM040103 As String = "開催登録　種別・支部選択画面"
        Public Const SCREEN_NAME_UC040104 As String = "組合大会通知 - 詳細画面"
        Public Const SCREEN_NAME_FM040104 As String = "開催登録　種別・支部選択画面"

        Public Const SCREEN_NAME_UC040201 As String = "争議行為画面"
        Public Const SCREEN_NAME_UC040202 As String = "争議行為通告 - 詳細画面"
        Public Const SCREEN_NAME_UC040203 As String = "労働協約第47条申し入れ - 詳細画面"
        Public Const SCREEN_NAME_UC040204 As String = "争議行為細部通告 - 詳細画面"
        Public Const SCREEN_NAME_UC040205 As String = "争議行為解除通知 - 詳細画面"
        Public Const SCREEN_NAME_UC040206 As String = "争議行為終結通知 - 詳細画面"
        Public Const SCREEN_NAME_FM040207 As String = "別紙情報登録画面画面"

        Public Const SCREEN_NAME_UC040301 As String = "時間内組合活動画面"
        Public Const SCREEN_NAME_UC040302 As String = "時間内組合活動 - 申請画面"
        Public Const SCREEN_NAME_FM040303 As String = "種類の選択画面"
        Public Const SCREEN_NAME_FM040304 As String = "中央員会の日程新規追加画面"
        Public Const SCREEN_NAME_FM040305 As String = "時間内組合活動申請可能残数一覧画面"
        Public Const SCREEN_NAME_UC040306 As String = "時間内組合活動 - 取消画面画面"

        Public Const SCREEN_NAME_UC040401 As String = "指名ストライキ画面"
        Public Const SCREEN_NAME_UC040402 As String = "指名ストライキ - 通告画面"
        Public Const SCREEN_NAME_UC040403 As String = "指名ストライキ - 一部解除画面"
        Public Const SCREEN_NAME_FM040404 As String = "争議行為通告番号選択画面"
        Public Const SCREEN_NAME_UC040405 As String = "氏名ストライキ - 通告画面"

        Public Const SCREEN_NAME_UC040501 As String = "当直スケジュール画面"
        Public Const SCREEN_NAME_UC040502 As String = "当直スケジュール - 詳細画面"
        Public Const SCREEN_NAME_FM040503 As String = "印刷対象日選択画面"

        Public Const SCREEN_NAME_UC040601 As String = "発信文書画面"
        Public Const SCREEN_NAME_FM040602 As String = "発信文書新規作成画面"            ' 発信文書新規作成
        Public Const SCREEN_NAME_FM040603 As String = "発信文書ファイル名入力画面"      ' 発信文書ファイル名入力
        Public Const SCREEN_NAME_FM040604 As String = "文書操作ウィンドウ画面"
        Public Const SCREEN_NAME_FM040605 As String = "コピー編集期選択画面"

        Public Const SCREEN_NAME_UC040701 As String = "住所変更画面"

        Public Const SCREEN_NAME_UC040801 As String = "UP・UJデータ画面"

        Public Const SCREEN_NAME_UC050101 As String = "賃金カット画面"
        Public Const SCREEN_NAME_UC050102 As String = "賃金カット - 月例・時間内画面"
        Public Const SCREEN_NAME_UC050103 As String = "賃金カット - 一時金画面"

        Public Const SCREEN_NAME_UC050201 As String = "日当計算画面"
        Public Const SCREEN_NAME_UC050202 As String = "委員日当計算 - 詳細画面"
        Public Const SCREEN_NAME_UC050203 As String = "中執日当計算 - 詳細画面"

        Public Const SCREEN_NAME_UC050301 As String = "源泉徴収画面"
        Public Const SCREEN_NAME_UC050302 As String = "源泉徴収 － 課税対象者画面"
        Public Const SCREEN_NAME_FM050303 As String = "集計対象年月の選択画面"
        Public Const SCREEN_NAME_UC050304 As String = "源泉徴収 － 課税対象者一時金画面"

        Public Const SCREEN_NAME_UC050401 As String = "収支予想画面"
        Public Const SCREEN_NAME_UC050402 As String = "収支予想 - 新規登録画面"
        Public Const SCREEN_NAME_UC050403 As String = "乗員計画状況 - 新規登録画面"
        Public Const SCREEN_NAME_FM050404 As String = "乗員計画新規登録画面"
        Public Const SCREEN_NAME_UC050405 As String = "分担金状況 - 新規登録画面"
        Public Const SCREEN_NAME_UC050406 As String = "予算登録 - 新規登録画面"
        Public Const SCREEN_NAME_UC050407 As String = "修正予算 － 新規登録画面"

        Public Const SCREEN_NAME_UC050501 As String = "銀行口座一覧出力画面"

        Public Const SCREEN_NAME_UC050601 As String = "日当データ出力画面"

        Public Const SCREEN_NAME_UC060101 As String = "選挙人名簿画面"

        Public Const SCREEN_NAME_UC080101 As String = "労金データ作成画面"
        Public Const SCREEN_NAME_FM080102 As String = "振込データ未作成の組合員画面"
        Public Const SCREEN_NAME_FM080103 As String = "振込データ新規作成画面"
        Public Const SCREEN_NAME_UC080104 As String = "労金データ作成 － 新規登録画面"

        Public Const SCREEN_NAME_UC080201 As String = "組合員口座情報画面"
        Public Const SCREEN_NAME_UC080202 As String = "組合員口座情報 - 詳細画面"

        Public Const SCREEN_NAME_FM090101 As String = "マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090102 As String = "定数マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090104 As String = "パスワードマスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090105 As String = "パスワードマスタメンテナンス - パスワードリセット画面"
        Public Const SCREEN_NAME_UC090106 As String = "委員会マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090107 As String = "委員会マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_UC090110 As String = "期マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090112 As String = "日当マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090114 As String = "中央執行昼食費マスタメンテナンス"
        Public Const SCREEN_NAME_UC090115 As String = "中央執行昼食費マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_UC090116 As String = "役員手当マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090117 As String = "役員手当マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_UC090118 As String = "課税率マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090120 As String = "切捨て金額マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090122 As String = "郵便番号マスタメンテナンス画面"
        Public Const SCREEN_NAME_FM090122 As String = "郵便番号マスタ更新中画面"
        Public Const SCREEN_NAME_UC090123 As String = "画面マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090125 As String = "メニューコントロールマスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090128 As String = "銀行マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090130 As String = "専従職員権限マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090133 As String = "会社マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090137 As String = "権限マスタメンテナンス画面"

        Public Const SCREEN_NAME_FM090201 As String = "財務管理マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090202 As String = "支出科目マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090203 As String = "支出科目マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_UC090204 As String = "組合費マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090205 As String = "組合費マスタメンテナンス - 新規登録画面"

        Public Const SCREEN_NAME_FM090301 As String = "管理部登録画面"
        Public Const SCREEN_NAME_UC090302 As String = "管理部名簿登録画面"
        Public Const SCREEN_NAME_UC090303 As String = "管理部マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090304 As String = "管理部マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_FM090305 As String = "画面操作権限画面"
        Public Const SCREEN_NAME_FM090306 As String = "画面操作権限詳細画面"
        Public Const SCREEN_NAME_UC090307 As String = "管理部名簿登録画面"

        Public Const SCREEN_NAME_FM090401 As String = "発信文書マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090402 As String = "発信文書宛先マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090403 As String = "発信文書宛先マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_UC090404 As String = "発信文書差出マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090405 As String = "発信文書差出マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_FM090406 As String = "部／委員会・役職選択画面画面"
        Public Const SCREEN_NAME_UC090407 As String = "発信文書委員会マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090408 As String = "発信文書委員会マスタメンテナンス - 新規登録画面"
        Public Const SCREEN_NAME_UC090409 As String = "委員会情報設定画面"
        Public Const SCREEN_NAME_UC090410 As String = "都道府県空港マスタメンテナンス画面"
        Public Const SCREEN_NAME_UC090411 As String = "都道府県空港マスタメンテナンス - 新規登録画面"

        Public Const SCREEN_NAME_UC100101 As String = "DGM管理画面"

        Public Const SCREEN_NAME_UC999999 As String = "複数エラーメッセージ表示画面"
        Public Const SCREEN_NAME_UCInfoMsg As String = "複数注意メッセージ表示画面"

        Public Const SCREEN_NAME_CLACCESSMDB As String = "データベース関連クラス"
        Public Const SCREEN_NAME_CLENCRYPT As String = "暗号化復号化関連クラス"
        Public Const SCREEN_NAME_CLMSG As String = "メッセージ関連クラス"
        Public Const SCREEN_NAME_MDCHK As String = "チェック関連モジュール"
        Public Const SCREEN_NAME_MDCOMMON As String = "共通モジュール"
        Public Const SCREEN_NAME_MDCONST As String = "定数モジュール"
        Public Const SCREEN_NAME_MDFILE As String = "ファイル関連モジュール"
        Public Const SCREEN_NAME_MDINFO As String = "情報関連モジュール"

        '===============================================================================
        '   定数ID
        '===============================================================================
        Public Const CONSTANT_ID_ADD_KIND As String = "ADD_KIND"                        ' 住所種別の内容
        Public Const CONSTANT_ID_APPLY_MEETINGLIST As String = "APPLY_MEETINGLIST"      ' 時間内会議名一覧の内容
        Public Const CONSTANT_ID_AREA_LOCAL As String = "AREA_LOCAL"                    ' 会社所属区分の内容
        Public Const CONSTANT_ID_BELONGING As String = "BELONGING"                      ' 組合支部区分の内容
        Public Const CONSTANT_ID_FIX_ADDRESS_INFO As String = "FIX_ADDRESS_INFO"        ' 固定所在地情報の内容
        Public Const CONSTANT_ID_INTERNATIONAL_KBN As String = "INTERNATIONAL_KBN"      ' 国内海外区分（国内・海外）
        Public Const CONSTANT_ID_LAW_POSITION As String = "LAW_POSITION"                ' 専従職員給与にて仕様
        Public Const CONSTANT_ID_LOS_POSITION As String = "LOS_POSITION"                ' 地位喪失理由区分の内容
        Public Const CONSTANT_ID_MODEL As String = "MODEL"                              ' 機種区分の内容
        Public Const CONSTANT_ID_OFFICER_NAME As String = "OFFICER_NAME"                ' 社長名の内容
        Public Const CONSTANT_ID_QUALIFICATION As String = "QUALIFICATION"              ' 資格（乗務員）の内容
        Public Const CONSTANT_ID_SEX As String = "SEX"                                  ' 性別
        Public Const CONSTANT_ID_STAF_KIND As String = "STAF_KIND"                      ' 組合員種別コード
        Public Const CONSTANT_ID_TEL_INFO As String = "TEL_INFO"                        ' 電話番号情報の内容
        Public Const CONSTANT_ID_UI_CIR_KIND As String = "UI_CIR_KIND"                  ' 組合大会通知 種別
        Public Const CONSTANT_ID_UI_CIR_STARTYEAR As String = "UI_CIR_STARTYEAR"        ' 組合大会通知 開催開始年
        Public Const CONSTANT_ID_UI_SHIBU As String = "UI_SHIBU"                        ' 支部選択肢
        Public Const CONSTANT_ID_SCD_SHIBU As String = "SCD_SHIBU"                      ' (スケジュール用)支部選択肢
        Public Const CONSTANT_ID_USER_STATUS As String = "USER_STATUS"                  ' 組合員ステータス区分
        Public Const CONSTANT_ID_WORK_PLACE As String = "WORK_PLACE"                    ' 職場区分の内容
        Public Const CONSTANT_ID_WORK_STATE As String = "WORK_STATE"                    ' 勤務状態の内容
        Public Const CONSTANT_ID_SENJYU_COMMITTEE_ID As String = "SENJYU_COMMITTEE_ID"  ' 専従職員所属委員会ID
        Public Const CONSTANT_ID_SENJYU_POST_ID As String = "SENJYU_POST_ID"            ' 専従職員所属役職ID
        Public Const CONSTANT_ID_DEPOSIT_ITEMS As String = "DEPOSIT_ITEMS"              ' 預金種目
        Public Const CONSTANT_APPLY_CLASSIFY As String = "APPLY_CLASSIFY"               ' 時間内組合活動種類
        Public Const CONSTANT_ID_APPLY_AREA As String = "APPLY_AREA"                    ' 申請支部

        '===============================================================================
        '   C1FlexGrid関連
        '===============================================================================
        ' グリッドセル内のテキストの配置
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTTOP As Byte = 0                     ' 水平方向には左揃えで、垂直方向には上揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER As Byte = 1                  ' 水平方向には左揃えで、垂直方向には中央揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTBOTTOM As Byte = 2                  ' 水平方向には左揃えで、垂直方向には下揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERTOP As Byte = 3                   ' 水平方向には中央揃えで、垂直方向には上揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER As Byte = 4                ' 水平方向には中央揃えで、垂直方向には中央揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERBOTTOM As Byte = 5                ' 水平方向には中央揃えで、垂直方向には下揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_RIGHTTOP As Byte = 6                    ' 水平方向には右揃えで、垂直方向には上揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_RIGHTCENTER As Byte = 7                 ' 水平方向には右揃えで、垂直方向には中央揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_RIGHTBOTTOM As Byte = 8                 ' 水平方向には右揃えで、垂直方向には下揃えでセルにテキストを配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_GENERALTOP As Byte = 9                  ' 数値は右揃え、他の値は左揃え、垂直方向には上揃えで配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_GENERALCENTER As Byte = 10              ' 数値は右揃え、他の値は左揃え、垂直方向には中央揃えで配置します。
        Public Const C1FLEXGRID_TEXT_ALIGN_ENUM_GENERALBOTTOM As Byte = 11              ' 数値は右揃え、他の値は左揃え、垂直方向には下揃えで配置します。

        ' スクロールバー設定
        Public Const C1FLEXGRID_SCROLLBARS_NONE As Byte = 0                             ' なし
        Public Const C1FLEXGRID_SCROLLBARS_HORIZONTAL As Byte = 1                       ' 横のみ
        Public Const C1FLEXGRID_SCROLLBARS_VERTICAL As Byte = 2                         ' 縦のみ
        Public Const C1FLEXGRID_SCROLLBARS_BOTH As Byte = 3                             ' 縦横

        ' フォーカス矩形外観
        Public Const C1FLEXGRID_FOCUS_RECT_ENUM_NONE As Byte = 0                        ' フォーカス矩形なし。
        Public Const C1FLEXGRID_FOCUS_RECT_ENUM_LIGHT As Byte = 1                       ' 細いフォーカス矩形（１ピクセル幅の点線）を表示します。これはデフォルトの設定です。  
        Public Const C1FLEXGRID_FOCUS_RECT_ENUM_HEAVY As Byte = 2                       ' 太いフォーカス矩形（２ピクセル幅の点線）を表示します。
        Public Const C1FLEXGRID_FOCUS_RECT_ENUM_SOLID As Byte = 3                       ' 実線のフォーカス矩形を表示します。矩形の色は、CellStyleCollection.Highlight.BackColor プロパティの値によって決定されます。 
        Public Const C1FLEXGRID_FOCUS_RECT_ENUM_RAISE As Byte = 4                       ' 浮き出たフォーカス矩形を表示します。
        Public Const C1FLEXGRID_FOCUS_RECT_ENUM_INSET As Byte = 5                       ' くぼんだフォーカス矩形を表示します。  

        ' 選択の種類指定
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_DEFAULT As Byte = 0                 ' ユーザーは、キーボードやマウスを使用して、連続するひとまとまりのセルを選択できます。ヘッダセルをクリックすると、行または列全体を選択できます。
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_CELL As Byte = 1                    ' ユーザーは、一度に１つのセルだけを選択できます。
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_CELLRANGE As Byte = 2               ' ユーザーは、キーボードやマウスを使用して、連続するひとまとまりのセルを選択できます。ヘッダセルをクリックしても選択範囲は変わりません。
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_ROW As Byte = 3                     ' ユーザーは、一度に１つの行を選択できます。
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_ROWRANGE As Byte = 4                ' 一度に連続する複数の行を選択できます。
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_COLUMN As Byte = 5                  ' ユーザーは、一度に１つの列を選択できます。
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_COLUMNRANGE As Byte = 6             ' 一度に連続する複数の列を選択できます。
        Public Const C1FLEXGRID_SELECTION_MODE_ENUM_LISTBOX As Byte = 7                 ' ［ CTRL ］キーを押しながらクリックして、連続しない複数の行を選択できます。

        '===============================================================================
        '   名称
        '===============================================================================
        Public Const KANJI_YOBI_NICHI As String = "日"                                  ' 曜日名称：日
        Public Const KANJI_YOBI_GETSU As String = "月"                                  ' 曜日名称：月
        Public Const KANJI_YOBI_KA As String = "火"                                     ' 曜日名称：火
        Public Const KANJI_YOBI_SUI As String = "水"                                    ' 曜日名称：水
        Public Const KANJI_YOBI_MOKU As String = "木"                                   ' 曜日名称：木
        Public Const KANJI_YOBI_KIN As String = "金"                                    ' 曜日名称：金
        Public Const KANJI_YOBI_DO As String = "土"                                     ' 曜日名称：土
        Public Const KANJI_YOBI As String = "曜日"                                      ' 曜日名称：曜日
        Public Const KANJI_JIKAN_HOUR As String = "時間"                                ' 名称：時間
        Public Const KANJI_JIKAN_MINUTE As String = "分"                                ' 名称：分
        Public Const KANJI_BUTTON_LABEL_TOUROKUKAKUNIN As String = "登録確認"           ' 名称：登録確認
        Public Const KANJI_BUTTON_LABEL_NAIYOUHENKOU As String = "内容変更"             ' 名称：内容変更
        Public Const KANJI_STRIKE_KIND_TSUKOKU As String = "争議行為通告"               ' 名称：争議行為通告
        Public Const KANJI_STRIKE_KIND_MOUSHIIRE As String = "申し入れ"                 ' 名称：申し入れ
        Public Const KANJI_STRIKE_KIND_SAIBU As String = "争議行為細部通告"             ' 名称：争議行為細部通告
        Public Const KANJI_STRIKE_KIND_KAIJYOU As String = "争議行為解除通知"           ' 名称：争議行為解除通知
        Public Const KANJI_STRIKE_KIND_SYUKETU As String = "争議行為終結通知"           ' 名称：争議行為終結通知
        Public Const KANJI_KAKKO_SUMI_LEFT As String = "【"                             ' 名称：左括弧
        Public Const KANJI_KAKKO_SUMI_RIGHT As String = "】"                            ' 名称：右括弧
        Public Const SCHEDULE_BUNRUI_INDEX_0 As String = "中執・大会等"                 ' 名称：中執・大会等
        Public Const SCHEDULE_BUNRUI_INDEX_1 As String = "専門部・委員会等"             ' 名称：専門部・委員会等
        Public Const SCHEDULE_BUNRUI_INDEX_2 As String = "産別"                         ' 名称：産別
        Public Const SCHEDULE_BUNRUI_INDEX_3 As String = "中執等"                       ' 名称：中執等
        Public Const SCHEDULE_BUNRUI_INDEX_4 As String = "大会"                         ' 名称：大会
        Public Const COMMITTEE_ID_CHUOU As String = "001"                               ' ID：中央委員会
        Public Const COMMITTEE_ID_SHIBU1 As String = "019"                              ' ID：支部委員会東京
        Public Const COMMITTEE_ID_SHIBU2 As String = "029"                              ' ID：支部委員会大阪

        '===============================================================================
        '   フォーマット
        '===============================================================================
        Public Const DATE_YYYYMMDD_FORMAT As String = "yyyy/MM/dd"                      ' 日付をYYYY/MM/DDにフォーマット
        Public Const DATE_YYYYMMDD_8_FORMAT As String = "yyyyMMdd"                      ' スラッシュなしのyyyyMMddフォーマット
        Public Const DATE_YYYYMMDD_KANJI_FORMAT As String = "yyyy年MM月dd日"            ' 日付をyyyy年MM月dd日にフォーマット
        Public Const DATE_HHMM_FORMAT As String = "HHmm"                                ' 時間をHH/MMにするためのフォーマット

        '===============================================================================
        '   権限
        '===============================================================================
        Public Const GRANT_VALID As String = "1"                                        ' 権限有り
        Public Const GRANT_VOID As String = "0"                                         ' 権限無し

        '===============================================================================
        '   文字列
        '===============================================================================
        ' ログ関連
        Public Const STR_LOG_BEGIN As String = " 処理開始"                              ' 処理開始ログ出力文字列
        Public Const STR_LOG_END As String = " 処理終了"                                ' 処理終了ログ出力文字列
        ' 種類
        Public Const STR_INFORMATION_NAME_OPEN As String = "開催"                       ' 開催
        Public Const STR_INFORMATION_NAME_UPDATE As String = "変更"                     ' 変更
        Public Const STR_INFORMATION_NAME_STOP As String = "中止"                       ' 中止
        ' 拡張子
        Public Const STR_EXTENSION_EXCEL As String = ".xls"                             ' Excel

        '===============================================================================
        '   ファイル名
        '===============================================================================
        Public Const FILE_CANCEL As String = "cancel.bmp"                               ' 取消画面
        Public Const FILE_RELEASE As String = "release.bmp"                             ' 解除画面

        '===============================================================================
        '   フラグ関連
        '===============================================================================
        ' 期
        Public Const FLG_OLD_PERIOD As Byte = 0                                         ' 1：最新期以外
        Public Const FLG_NEW_PERIOD As Byte = 1                                         ' 0：最新期

    End Module

End Namespace
#End Region