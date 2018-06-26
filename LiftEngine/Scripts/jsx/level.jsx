var Level = React.createClass({

    handleUp: function() {
        this.props.addStopFunc(this.props.levelNumber, 1);
    },

    handleDown: function() {
        this.props.addStopFunc(this.props.levelNumber, -1);
    },
    
    render: function() {
        var level = this.props.data;
        return (
            <tr>
                <td>         
                    {this.props.allowUp === true &&       
                    (<div>
                        <input type="button" 
                               onClick={this.handleUp}
                               className="btn btn-primary" 
                               value="Up" 
                               disabled={level.SummonedUp}/>
                    </div>)}

                    {this.props.allowDown === true &&
                    (<div>
                        <input type="button" 
                               onClick={this.handleDown}
                               className="btn btn-primary" 
                               value="Down" 
                               disabled={level.SummonedDown}/>
                    </div>)}
                </td>
                <td className="level">{level.Name}</td>
            </tr>);
    }
})