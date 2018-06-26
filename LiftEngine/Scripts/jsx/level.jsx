var Level = React.createClass({

    handleUp: function() {
        this.props.addStopFunc(this.props.levelNumber, directionEnum.Up);
    },

    handleDown: function() {
        this.props.addStopFunc(this.props.levelNumber, directionEnum.Down);
    },
    
    render: function() {
        var level = this.props.data;
        return (
            <tr className="level">
                <td>     
                    <div className="summons">
                    {this.props.allowUp === true &&       
                    (<div>
                        <button onClick={this.handleUp}
                                className="btn btn-primary" 
                                disabled={level.SummonedUp}>
                            <span className="glyphicon glyphicon-chevron-up"/>
                        </button>
                    </div>)}

                    {this.props.allowDown === true &&
                    (<div>
                        <button onClick={this.handleDown}
                                className="btn btn-primary" 
                                disabled={level.SummonedDown}>
                             <span className="glyphicon glyphicon-chevron-down"/>
                         </button>
                    </div>)}
                    </div>
                </td>
                <td ><div className="level-name">{level.Name}</div></td>
            </tr>);
    }
})