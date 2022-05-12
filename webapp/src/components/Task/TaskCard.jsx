import React, { Component } from 'react';
import PopupEditTask from '../../modals/Task/PopupEditTask';
import PopupDeleteTask from '../../modals/Task/PopupDeleteTask';
import PopupToggleStatus from '../../modals/Task/PopupToggleStatus';

class TaskCard extends Component {
    constructor(props) {
        super(props);
        this.state = {
            modalEdit: false,
            modalDelete: false,
            modalToggle: false,
            value: 0
        }
    }

    render() { 
        const task = this.props.task;

        const toLocalDate = (date) => {
            const d = new Date(date);
            return d.toLocaleString();
        }

        const handleEdit = async (taskObj) => {
            await this.props.updateTask(taskObj);
        }

        const handleDelete = async (taskObj) => {
            await this.props.deleteTask(taskObj);
        }

        const toggleStatus = async (taskObj) => {
            await this.props.toggleTask(taskObj);
            this.setState({value: this.state.value + 1});
        }

        return ( 
            <div className='card-wrapper'>
                <div className='card-top'></div>
                <div className='task-holder text-white'>
                    <h4 className='text-white'>{task.title}</h4>
                    <h6 className='text-white'>{"Due: " + toLocalDate(task.dueDate)}</h6>
                    <h6 className='text-white' >{"Created: " + toLocalDate(task.createDate)}</h6>
                    <h6 style={{"color": task.isCompleted === true ? "green" : "red"}}>
                    {(task.isCompleted === true ? "Completed" : "Not Completed") + (task.isCompleted === true ? " - " + toLocalDate(task.completedDate) : "")}
                    </h6>
                    <hr></hr>
                    <h6 className='text-white'>{task.description}</h6>

                    <div style={{"position": "absolute", "bottom": "-20px", "left": "10px", "display": "flex", "flexDirection": "row", "justifyContent": "space-evenly"}}>
                        <button className='btn' style={{"color": "green"}} onClick={() => this.setState({modalEdit: true})}>Edit</button>
                        <button className='btn' style={{"color": "red"}} onClick={() => this.setState({modalDelete: true})}>Delete</button>
                        <button className='btn' style={{"color": "white"}} onClick={async () => this.setState({modalToggle: !this.state.modalToggle})}>Toggle Status</button>
                    </div>
                </div>

                <PopupEditTask task={task} handleTask={handleEdit} modal={this.state.modalEdit} toggle={() => this.setState({modalEdit: !this.state.modalEdit})} />
                <PopupDeleteTask task={task} handleTask={handleDelete} modal={this.state.modalDelete} toggle={() => this.setState({modalDelete: !this.state.modalDelete})} />
                <PopupToggleStatus task={task} handleTask={toggleStatus} modal={this.state.modalToggle} toggle={() => this.setState({modalToggle: !this.state.modalToggle})} />
            </div>
        );
    }
}
 
export default TaskCard;