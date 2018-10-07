var Levels = React.createClass({

    
    renderLevels: function() {
        // reverse levels so ground is at the bottom
        var levelsTopToBottom = this.props.data.slice();
        levelsTopToBottom = levelsTopToBottom.reverse();

        // calcuate level number as it is no longer the index
        var levelCount = levelsTopToBottom.length;
        var levels = levelsTopToBottom.map((level, i) => {
            var levelNumber = levelCount - i - 1;
            return (
                <Level key = { i }
                       levelNumber = { levelNumber }
                       data = { level }
                       allowUp = { i !== 0 }
                       allowDown = { i !== levelsTopToBottom.length - 1 }
                       isCurrentLevel = {this.props.currentLevel === levelNumber}
                       addStopFunc = { this.props.addStopFunc } />);
        });

        return levels;
    },

    render: function () {
        return (
            <table>
                <thead>
                <tr>
                    <td className="col-sm-6"></td>
                    <td className="col-sm-6"></td>
                </tr>
                </thead>
                <tbody>{this.renderLevels()}</tbody>
            </table>);
    }

})