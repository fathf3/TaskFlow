import React, { useState } from 'react';
import { Header } from '../components/Header';
import { Sidebar } from '../components/Sidebar';
import { Dashboard } from '../components/Dashboard';
import { ProjectList } from '../components/ProjectList';
import { TaskBoard } from '../components/TaskBoard';
import { UserManagement } from '../components/UserManagement';
import { Toaster } from '../components/ui/toaster';
import Login from '../components/Login';

const Index = () => {
  const [activeView, setActiveView] = useState('dashboard');
  const [sidebarOpen, setSidebarOpen] = useState(true);

  const renderActiveView = () => {
    switch (activeView) {
      case 'dashboard':
        return <Dashboard />;
      case 'projects':
        return <ProjectList />;
      case 'tasks':
        return <TaskBoard />;
      case 'users':
        return <UserManagement />;
      default:
        return <Dashboard />;
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 to-blue-50">
      <Header 
        onMenuClick={() => setSidebarOpen(!sidebarOpen)}
        sidebarOpen={sidebarOpen}
      />
      
      <div className="flex">
        <Sidebar 
          isOpen={sidebarOpen}
          activeView={activeView}
          onViewChange={setActiveView}
        />
        
        <main className={`flex-1 transition-all duration-300 ${sidebarOpen ? 'ml-64' : 'ml-16'} pt-16`}>
          <div className="p-6">
            {renderActiveView()}
          </div>
        </main>
      </div>
      
      <Toaster />
    </div>
  );
};

export default Index;
