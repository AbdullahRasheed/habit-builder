import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './LoginPage';
import SignupPage from './SignupPage';
import CreateHabit from './components/CreateHabit';
import axios from 'axios';
import HabitListPage from './HabitListPage';

function App() {
  const auth = localStorage.getItem("habitTrackerAccessTok");
  if(auth){
    axios.defaults.headers.common['Authorization'] = "Bearer " + auth;
  }

  return (
    <Router>
      <Routes>
        <Route exact path='/login' element={<LoginPage />} />
        <Route exact path='/signup' element={<SignupPage />} />
        <Route exact path='/devtest' element={<HabitListPage />} />
      </Routes>
    </Router>
  );
}

function TitlePage() {
  // Build. Better. Habits.
  // Login     Signup
  return (
    <div className="title-page">
      
    </div>
  )
}

export default App;
