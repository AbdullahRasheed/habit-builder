import { useEffect, useState } from 'react';
import './HabitListPage.css';
import {Get} from './RESTRequest';
import CreateHabit from './components/CreateHabit';

function HabitListPage(){
    const [render, setRender] = useState(0); // is this really the best way to do it?
    function consume(response){
        console.log("consumed");
        setRender(prev => prev + 1);
    }

    return (
        <div>
            <div>
                <CreateHabit onResponse={consume}/>
            </div>
            <HabitList />
        </div>
    )
}

function HabitList(){
    const [habitList, setHabitList] = useState();
    useEffect(() => {
        const fetchInfo = async () => {
            await Get("habit/info")
                .then(response => {
                    setHabitList(response.data.map((habit, i) => {
                        return <HabitListItem key={i} {...habit} />
                    }));
                });
        }

        fetchInfo(); 
    }, []);

    if(habitList === undefined) return <p>Loading...</p>

    return (
        habitList
    )
}

function HabitListItem(properties){
    async function loadHabit(){
        // use properties.id to redirect
        // to the HabitPage
    }

    return (
        <div>
            <h3>{properties.name}</h3>
            <p>{properties.description}</p>
        </div>
    )
}

export default HabitListPage;