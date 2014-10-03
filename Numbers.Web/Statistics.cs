using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace Numbers.Web
{
    public class Statistics
    {
        private const string SolvedCountConfigurationKey = "SolvedCount";

        private IConfiguration configuration;

        private int totalSolvedCount;
        private int TotalSolvedCount
        {
            get { return totalSolvedCount; }
            set
            {
                totalSolvedCount = value;
                configuration.SetValue(SolvedCountConfigurationKey, totalSolvedCount.ToString());
            }
        }

        private int sessionSolvedCount;
        private DateTime currentGameStart;
        private Game currentGame;

        public Statistics(IConfiguration configuration)
        {
            this.configuration = configuration;

            int storedSolvedCount;
            totalSolvedCount = Int32.TryParse(configuration.GetValue(SolvedCountConfigurationKey), out storedSolvedCount) ? storedSolvedCount : 0;
        }

        public void ReportSessionStart()
        {
            sessionSolvedCount = 0;
        }

        public void ReportSessionEnd()
        {
            ReportGameEnd();

            if (sessionSolvedCount > 0)
            {
                SendEvent("Session", "Solved", "Count", sessionSolvedCount);
                SendEvent("Total", "Solved", "Count", totalSolvedCount);
            }
        }

        public void ReportGameStart(Game game)
        {
            currentGame = game;
            currentGameStart = DateTime.Now;
        }

        public void ReportGameEnd()
        {
            if (currentGame == null)
            {
                return;
            }

            if (currentGame.IsSolved && currentGame.HintsCount <= 3)
            {
                sessionSolvedCount++;
                TotalSolvedCount++;
            }

            string generalCategory = "Game";
            string solutionsCategory = GetSolutionsCategory(currentGame.SolutionsCount);
            string targetCategory = GetTargetCategory(currentGame.TargetValue);

            int gameDuration = DateTime.Now - currentGameStart;

            string action;
            bool skipped = !currentGame.IsSolved && (currentGame.StepsCount == 0 || gameDuration < 10000);

            if (currentGame.IsSolved)
            {
                action = currentGame.HintsCount <= 3 ? "Solved" : "Hinted";
            }
            else
            {
                action = skipped ? "Skipped" : "Gave up";
            }

            if (gameDuration < 1800000 && !skipped)
            {
                SendTiming(generalCategory, action, "Duration", gameDuration);
                SendTiming(solutionsCategory, action, "Duration", gameDuration);
            }

            SendEvent(generalCategory, action);
            SendEvent(skipped ? targetCategory : solutionsCategory, action);

            currentGame = null;
        }

        private static string GetSolutionsCategory(int solutionsCount)
        {
            int rangeSize;

            if (solutionsCount < 8)
            {
                rangeSize = 1;
            }
            else if (solutionsCount < 16)
            {
                rangeSize = 2;
            }
            else if (solutionsCount < 32)
            {
                rangeSize = 4;
            }
            else if (solutionsCount < 64)
            {
                rangeSize = 8;
            }
            else
            {
                rangeSize = 16;
            }

            return rangeSize == 1 ?
                String.Format("Game solutions {0}", solutionsCount) :
                String.Format("Game solutions {0}-{1}", rangeSize * (solutionsCount / rangeSize), rangeSize * (solutionsCount / rangeSize + 1) - 1);
        }

        private static string GetTargetCategory(int targetValue)
        {
            const int rangeSize = 50;
            return String.Format("Game target {0}-{1}", rangeSize * (targetValue / rangeSize), rangeSize * (targetValue / rangeSize + 1) - 1);
        }

        [InlineCode("ga('send', 'event', {{ 'eventCategory': {category}, 'eventAction': {action} }});")]
        private static void SendEvent(string category, string action)
        {
            //
        }

        [InlineCode("ga('send', 'event', {{ 'eventCategory': {category}, 'eventAction': {action}, 'eventLabel': {label}, 'eventValue': {value} }});")]
        private static void SendEvent(string category, string action, string label, int value)
        {
            //
        }

        [InlineCode("ga('send', 'timing', {{ 'timingCategory': {category}, 'timingVar': {variableName}, 'timingValue': {milliseconds}, 'timingLabel': {label} }});")]
        private static void SendTiming(string category, string variableName, string label, int milliseconds)
        {
            //
        }
    }
}
