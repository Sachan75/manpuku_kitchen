using System.Collections.Generic;
using UnityEngine;

namespace manpuku_kitchen.Utils
{
    public class Scorer
    {

        // 連鎖ボーナステーブル
        private static readonly int[] chainBonus = { 0, 0, 8, 16, 32, 64, 96, 128, 160, 192, 224,
        256, 288, 320, 352, 384, 416, 448, 480, 512 };
        // 連結数ボーナステーブル
        private static readonly int[] stickBonus =
            { 0, 0, 0, 0, 0, 2, 3, 4, 5, 6, 7};
        // 同時消しボーナステーブル
        private static readonly int[] colorBonus = { 0, 0, 3, 6, 12, 24 };

        public static int CountScore(Dictionary<Ingredients, List<int>> ingredientsCount, int chainCount)
        {
            //連鎖ボーナス
            int chain = chainBonus[chainCount];

            //連結ボーナス
            int stick = 0;
            // 総素材数
            int totalCount = 0;
            foreach (List<int> value in ingredientsCount.Values)
            {
                var count = value.Count;
                if (count < 4)
                {
                    continue;
                }
                totalCount += count;

                if (count >= 11) stick += 10;
                else stick += stickBonus[count];
            }

            int colorCount = ingredientsCount.Count;

            //色数ボーナス
            int color = colorBonus[colorCount];

            int total = Mathf.Max(chain + stick + color, 1);

            int score = totalCount * 10 * total;

            if (score > 0)
            {
                Debug.Log(chainCount + "連鎖！");
            }

            return score;
        }
    }
}