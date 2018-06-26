var Lift = React.createClass({

    getInitialState: function() {
        return {
            loaded: false,
            data: {}
        }
    },

    componentDidMount: function() {
        this.handleGetLift();
    },

    handleGetLift: function() {
        $.ajax({
            url: this.props.api,
            success: function(result) {
                this.setState({
                    loaded: true,
                    data: result
                });
            }.bind(this),
            datatype: "json"
        });
    },

    handleAddStop: function (level, direction) {
        var data = {
            "level": level,
            "direction": direction
        };
        $.ajax({
            url: this.props.apiStops,
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json",
            success: function() {
                // refresh the lift
                this.handleGetLift();
            }
        });
    },

    renderLevels: function() {
        // reverse levels so ground is at the bottom
        var levelsTopToBottom = this.state.data.Levels.reverse();

        // calcuate level number as it is no longer the index
        var levelCount = levelsTopToBottom.length;
        var levels = levelsTopToBottom.map((level, i) =>
            <Level key={i}
                   levelNumber={levelCount-i-1}
                   data={level} 
                   allowUp={i !== 0} 
                   allowDown={i !== levelsTopToBottom.length - 1 }
                   addStopFunc={this.handleAddStop}/>);

        return levels;
    },

    render: function () {
        return (this.state.loaded === true
            ? <table>
                <thead>
                    <tr>
                        <td className="col-sm-6"></td>
                        <td className="col-sm-6"></td>
                    </tr>
                </thead>
                <tbody>{this.renderLevels()}</tbody>
            </table>
            : <div>"Loading...."</div>);
    }
})