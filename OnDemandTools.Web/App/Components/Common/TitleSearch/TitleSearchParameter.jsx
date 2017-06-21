import React from 'react';
import Select from 'react-select';
import { FormGroup } from 'react-bootstrap';
class TitleSearchParameter extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        var options = [];
        if (this.props.parameters != null && this.props.parameters.length > 0) {
            for (var i = 0; i < this.props.parameters.length; i++) {
                var filter = this.props.parameters[i];
                var option = {};
                option.value = filter.name;
                option.label = filter.name + " (" + filter.count + ")";
                options.push(option);
            }
        }

        return (
            <FormGroup controlId="content">
                <Select simpleValue className="destination-select-control" options={options}
                    clearable={true}
                    searchable={true}
                    placeholder={this.props.name} />
            </FormGroup>
        )
    }
}

export default TitleSearchParameter