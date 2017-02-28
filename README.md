# Image-use-K-means</br>

#K-means</br>
K-means屬於一種分群演算法，可以簡單且快速的將分散的資料依照設定群數(K)分群。</br>
</br>
首先來看看討人厭的數學公式</br>

![center](http://latex.codecogs.com/gif.latex? \\Large arg_{s}min \\displaystyle\\sum_{i=1}^{k} \\displaystyle\\sum_{x\\in S_{i}} \\left\\|x- \\mu _{i}\\rught\\| ^2 ) </br>
其實公式也不算是太複雜，白話的來說這些公式就是想取得每一群之中所有點到中心點的距離最小。</br>
</br>
</br>
以步驟來說明的話就是:</br>
1.首先先隨機取得K(分群數)個座標</br></br>
2.計算每個目前資料分佈離哪個座標比較近，就先歸類到該座標的群組去</br></br>
3.計算每群新的中心座標，並取代原先座標</br></br>
</br>
</br>
接下來就是一直重複(2).(3)步驟直到每個群的成員不再變動、每個群中心位置都固定下來</br>
</br>
</br>
如果還是不明白的話讓我們來看看下面這個gif圖</br>
<img src="http://i.giphy.com/l44QkFPuIUHLk6raU.gif" width="971" height="604" border="0" /></a><br />
到這裡相信對k-means有比較認識了，但這個方法也是存在著缺點的</br>
怎麼說呢?  直接看看下面這個例子</br>
<img src="http://i.imgur.com/FT9iswv.png" /></br>
萬一一開始紅色的點被分在最外圍，那結果會是怎樣的呢?</br>
</br>
這個能就會是最後分類的結果，因為紅色群的中心一直無法靠近其他群</br>
<img src="http://i.imgur.com/Y2pI4Bx.png"></br>
</br>
這也展示了這個演算法的缺點，就是因為一開始的群中心點是"隨機"而來的</br>
所以最終的結果每次都不一定會"完全一樣"</br>
</br>
</br>
</br>而將kmeans應用在圖片上就能有以下的效果
</br>
##Demo</br>
</br>
K=3</br>
</br>
<img src="http://i.imgur.com/ev9v8nJ.png" />
</br>可以看到圖片大致上被分為K群(K種顏色)並重新繪製於影像上
</br>
</br>下面GIF可以呈現出每次分群的結果，可以從中看出分群的過程
</br>
<img src="http://i.giphy.com/3o7TKRnJVMH0PLNUQ0.gif">
</br>
</br>
</br>
</br>
</br>
</br>
##參考文章K-means
https://zh.wikipedia.org/wiki/K-%E5%B9%B3%E5%9D%87%E7%AE%97%E6%B3%95
