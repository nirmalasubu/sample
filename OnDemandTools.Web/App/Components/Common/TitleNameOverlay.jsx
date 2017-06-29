import React from 'react';
import { connect } from 'react-redux';
import { searchByTitleIds } from 'Actions/TitleSearch/TitleSearchActions';

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

    fetchAndUpdateTitles() {

        var titleIds= this.props.data;

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

    render() {
        if (this.state.isProcessing) {
            return <div>Loading...</div>
        }
        else if (this.state.titles.length == 0) {
            return <div></div>
        }
        return (
            <div>
                {this.state.titles[0].titleName}
            </div>
        )
    }
}

export default TitleNameOverlay