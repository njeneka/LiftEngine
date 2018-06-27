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

    handleTravel: function () {
        $.ajax({
            url: this.props.apiTravel,
            type: "POST",
            contentType: "application/json",
            success: function () {
                // refresh the lift
                this.handleGetLift();
            }.bind(this)
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
            }.bind(this)
        });
    },

    render: function () {
        return (this.state.loaded !== true
            ? <div>"Loading...."</div>
            : <div className="row">
                <div className="col-sm-4 col-sm-offset-4">
                    <h2>Building Levels</h2>
                    <Levels data = {this.state.data.Levels}
                            currentLevel = {this.state.data.CurrentLevel}
                            summonsUp = {this.state.data.SummonsUp}
                            summonsDown = {this.state.data.SummonsDown}
                            addStopFunc = { this.handleAddStop } />
                </div>
                <div className="col-sm-4"> 
                    <h2>Select Level to Disembark</h2>
                    <div className="row">
                        <div className="col-sm-1 col-sm-offset-5">
                            <LiftPanel data={this.state.data.Levels}
                                       disembark={this.state.data.Disembark}
                                       addStopFunc={ this.handleAddStop } />
                        </div>
                    </div>

                    <div className="row travel">
                        <div className="col-sm-4 col-sm-offset-1">
                            <button onClick={() => this.handleTravel()}
                                    className="btn btn-primary"
                                    disabled={this.state.data.CurrentDirection === directionEnum.Any}>
                                <span className="travel">Travel to Next Stop</span>
                            </button>
                        </div>
                    </div>

                    <h4>Stop History*</h4>
                    <div>* 0 based index of level: G=0, L1=1, etc</div>
                    <div>{this.state.data.StopHistoryDisplay}</div>
                </div>
              </div>);
    }
})