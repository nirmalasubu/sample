import React from 'react';
import { connect } from 'react-redux';
import { searchByTitleIds } from 'Actions/TitleSearch/TitleSearchActions';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
@connect((store) => {
    return {
    };
})
class TitleNameOverlay extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            titleIds: [],
            titles: [],
            isProcessing: false
        }
    }

    //<summary>
    ///  fetchAndUpdateTitles when component is initially loaded
    ///</summary>
    fetchAndUpdateTitles() {

        var titleIds = this.props.data;
        this.setState({
            titlesIds: this.props.data

        });
        let searchPromise = searchByTitleIds(titleIds);

        searchPromise.then(message => {
            this.setState({ titles: message, isProcessing: false });
        })
        searchPromise.catch(error => {
            console.error(error);
            this.setState({ titles: [], isProcessing: false });
        });
    }

    componentDidMount() {
        this.setState({
            titlesIds: this.props.data,
            isProcessing: true
        });

        this.fetchAndUpdateTitles();
    }



    ///<summary>
    /// update the state when  titles are added  or removed with new properties
    ///</summary>
    fetchAndUpdateTitleswithNextProps(titleIds) {

        var titleIds = titleIds;
        this.setState({
            titlesIds: titleIds

        });
        let searchPromise = searchByTitleIds(titleIds);

        searchPromise.then(message => {
            this.setState({ titles: message, isProcessing: false });
        })
        searchPromise.catch(error => {
            console.error(error);
            this.setState({ titles: [], isProcessing: false });
        });
    }

    //<summary>
    /// after component is mounted . update the state when  titles are added  or removed
    ///</summary>
    componentWillReceiveProps(nextProps) {

        if (nextProps.data.length != this.props.data.length) {
            console.log("fetchAndUpdateTitles");
            this.fetchAndUpdateTitleswithNextProps(nextProps.data);
        } else {
            for (var i = 0; i < this.props.data.length; i++) {
                if (nextProps.data[i] != this.props.data[i]) {
                    console.log("fetchAndUpdateTitles in else");
                    this.fetchAndUpdateTitleswithNextProps(nextProps.data);
                    return false;
                }
            }
        }
    }

    render() {

        var titleNames = [];
        var sortedTitleRows = [];
        var titleText = "";
        if (this.state.titles.length > 0) {
            for (var i = 0; i < this.state.titles.length; i++) {
                titleNames.push(this.state.titles[i].titleName)
            }
        }
        // sort the title names
        if (titleNames.length > 0) {
            titleNames.sort();
            titleText = titleNames[0];
            for (var i = 0; i < titleNames.length; i++) {
                sortedTitleRows.push(<p key={i.toString()}> {titleNames[i]} </p>)
            }
        }

        const popoverLeft = (
            <Popover id="popover-positioned-left" title="Titles/Series">
                <div class="TitleOverlay-height"> {sortedTitleRows} </div>
            </Popover>
        );

        if (this.state.isProcessing) {
            return <div>Loading...</div>
        }
        else if (this.state.titles.length == 0) {
            return <div></div>
        }
        else if (this.state.titles.length == 1 || this.props.disableOverlay) {
            return (
                <div>
                    {titleText}
                </div>
            )
        }
        else {
            return (
                <OverlayTrigger trigger={['click']} rootClose placement="left" overlay={popoverLeft}>
                    <div className="cursorPointer" title="click to view more Title/Series">
                        {titleText} <i class="fa fa-ellipsis-h"></i>
                    </div>
                </OverlayTrigger>)
        }
    }
}

export default TitleNameOverlay