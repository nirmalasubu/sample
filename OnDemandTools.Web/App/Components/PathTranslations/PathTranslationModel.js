/// <summary>
/// Standard model for path translation related data
/// </sumamry>

class PathTranslationModel {
    constructor(pathTranslations=[]) {
        // always initialize all instance properties
        this.pathTranslations = pathTranslations;
    }
    getPathTranslations() {
        return this.pathTranslations;
    }
}


module.exports = PathTranslationModel;