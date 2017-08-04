import Moment from 'moment';

/// <summary>
/// Standard model for path translation related data
/// </sumamry>

const PathTranslationModel =
{
  id: "",
  source: {
    baseUrl: "",
    brand: "",
    protectionType: "",
    urlType: ""
  },
  target: {
    baseUrl: "",
    brand: "",
    protectionType: "",
    urlType: ""
  },
  modifiedBy: "",
  modifiedDateTime: new Moment(),
  createdBy: "",
  createdDateTime: new Moment()
}

module.exports = PathTranslationModel;