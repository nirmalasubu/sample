using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    /// <summary>
    ///  Base class to transform Deliverables and Properties Tokens of airing destination
    /// </summary>
    public class Formatter
    {
        #region CONSTANTS
        private const string HdPattern = @"{IFHD=([\w\W\d ]+)ELSE=([\w\W\d ]*)}";

        private const string AiringId = "{AIRING_ID}";
        private const string AiringName = "{AIRING_NAME}";

        private const string Brand = "{BRAND}";
        private const string Episode = "{TITLE_EPISODE_NUMBER}";

        private const string AiringStorylineLong = "{AIRING_STORYLINE_LONG}";
        private const string AiringStorylineShort = "{AIRING_STORYLINE_SHORT}";
        private const string TitleStorylinePattern = @"{TITLE_STORYLINE([\w\W\d ]+)}";
        #endregion

        protected readonly Airing Airing;

        #region CONSTRUCTOR

        public Formatter()
        {

        }

        public Formatter(Airing airing)
        {
            Airing = airing;
        }
        #endregion

        #region PRIVATE METHODS

        public string Format(string value)
        {
            value = Airing == null
                   ? value.Replace(AiringId, string.Empty)
                   : value.Replace(AiringId, Airing.AssetId);

            value = Airing == null
                   ? value.Replace(AiringName, string.Empty)
                   : value.Replace(AiringName, Airing.Name);

            value = Airing == null
                 ? value.Replace(Brand, string.Empty)
                 : value.Replace(Brand, Airing.Network);

            value = Airing == null
                ? value.Replace(AiringStorylineLong, string.Empty)
                : value.Replace(AiringStorylineLong, FormatAiringStoryLineLong(Airing));

            value = Airing == null
                ? value.Replace(AiringStorylineShort, string.Empty)
                : value.Replace(AiringStorylineShort, FormatAiringStoryLineShort(Airing));

            value = Airing == null
               ? value.Replace(Episode, string.Empty)
               : value.Replace(Episode, FormatEpisodeNumber(Airing.FlowTitleData));

            value = FormatTitleStorylines(value);

            value = FormatHd(Airing, value);

            return value;
        }

        #endregion

        #region PRIVATE METHODS

        private string FormatAiringStoryLineShort(Airing airing)
        {
            if(!string.IsNullOrEmpty(Airing.Title.StoryLine.Short))
            {
                return Airing.Title.StoryLine.Short;
            }

            var primaryTitleId = airing.Title.TitleIds.FirstOrDefault(t => t.Primary);
            if (primaryTitleId != null)
            {
                var primaryTitle = Airing.FlowTitleData.First(t => t.TitleId == int.Parse(primaryTitleId.Value));
                var storyline = primaryTitle.Storylines.FirstOrDefault(s => s.Type == "Short (245 Characters)");
                return (storyline == null) ? string.Empty : storyline.Description;
            }
            return string.Empty;
        }

        private string FormatAiringStoryLineLong(Airing airing)
        {
            if (!string.IsNullOrEmpty(Airing.Title.StoryLine.Long))
            {
                return Airing.Title.StoryLine.Long;
            }
            var primaryTitleId = airing.Title.TitleIds.FirstOrDefault(t => t.Primary);

            if (primaryTitleId != null)
            {
                var primaryTitle = Airing.FlowTitleData.First(t => t.TitleId == int.Parse(primaryTitleId.Value));
                var storyline = primaryTitle.Storylines.FirstOrDefault(s => s.Type == "Turner External");

                return (storyline == null) ? string.Empty : storyline.Description;
            }

            return string.Empty;
        }

        private string FormatEpisodeNumber(IEnumerable<Alternate.Title.Title> titles)
        {
            var episodeNumber = new StringBuilder();

            foreach (var title in titles)
            {
                if (episodeNumber.Length == 0)
                {
                    episodeNumber.Append(title.EpisodeNumber);
                }
                else
                {
                    episodeNumber.Append("/");
                    episodeNumber.Append(title.EpisodeNumber);
                }
            }

            return episodeNumber.ToString();
        }

        private string FormatTitleStorylines(string value)
        {
            var match = Regex.Match(value, TitleStorylinePattern);

            if (Airing == null)
                return Regex.Replace(value, TitleStorylinePattern, string.Empty);

            if (!match.Success)
                return value;

            var type = match.Groups[1].Value.Substring(1, match.Groups[1].Value.Length - 2);

            return Regex.Replace(value, TitleStorylinePattern, BuildStorylineFrom(Airing.FlowTitleData, type));
        }

        private string FormatHd(Airing airing, string value)
        {
            if (!Regex.IsMatch(value, HdPattern))
                return value;

            return airing == null
                ? Regex.Replace(value, HdPattern, "")
                : ReplaceHdTokens(value);
        }

        private string ReplaceHdTokens(string value)
        {
            var match = Regex.Match(value, HdPattern);

            var newValue = (Airing.Flags.Hd)
                    ? Regex.Replace(value, HdPattern, ReplaceParentheses(match.Groups[1].Value))
                    : Regex.Replace(value, HdPattern, ReplaceParentheses(match.Groups[2].Value));

            return newValue;
        }

        private string ReplaceParentheses(string value)
        {
            return value.Substring(1, value.Length - 2);
        }

        private string BuildStorylineFrom(IEnumerable<Alternate.Title.Title> titles, string type)
        {
            var storyline = new StringBuilder();

            foreach (var storylineOption in titles
                .SelectMany(title => title.Storylines
                .Where(storylineOption => storylineOption.Type == type)))
            {
                if (storyline.Length == 0)
                {
                    storyline.Append(storylineOption.Description);
                }
                else
                {
                    storyline.Append(" / ");
                    storyline.Append(storylineOption.Description);
                }
            }

            return storyline.ToString();
        }

        #endregion
    }
}
