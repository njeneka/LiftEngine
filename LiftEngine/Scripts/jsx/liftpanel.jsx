var LiftPanel = React.createClass({
    
    renderLevels: function() {
        // reverse levels so ground is at the bottom
        var levelsTopToBottom = this.props.data.slice();
        levelsTopToBottom = levelsTopToBottom.reverse();

        // calcuate level number as it is no longer the index
        var levelCount = levelsTopToBottom.length;
        var levels = levelsTopToBottom.map((level, i) => {
            var levelNumber = levelCount - i - 1;
            return (
                <div key={level.Name} className="row">
                    <div className="col btn-lift-panel">
                        <button onClick={() => this.props.addStopFunc(levelNumber, directionEnum.Any)}
                                className="btn btn-primary"
                                disabled={this.props.disembark.includes(levelNumber) === true}>
                            {level.Name}
                        </button>
                    </div>
                </div>);
        });

        return levels;
    },

    render: function () {
        return (
            <div>
                {this.renderLevels()}
            </div>);
    }
})