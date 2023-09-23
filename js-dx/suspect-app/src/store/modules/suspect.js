import api from '@/services/api'

const state = {
    isLoadingSuspects: false,
    isLoadSuspects: false,
    suspectsItems: [],
    isLoadSuspect: false,
    suspect: null,
    messageError: ''
}

const getters = {
    isLoadingSuspects: state => state.isLoadingSuspects,
    isLoadSuspects: state => state.isLoadSuspects,
    suspectsItems: state => state.suspectsItems,
    isLoadSuspect: state => state.isLoadSuspect,
    suspect: state => state.suspect,
    messageError: state => state.messageError
}

const mutations = {
    getSuspectsItemsStarted (state) {
        state.isLoadingSuspects = true
        state.isLoadSuspects = false
        state.suspectsItems = []
    },
    getSuspectsItemsSuccess (state, items) {
        state.isLoadingSuspects = false
        state.isLoadSuspects = true
        state.suspectsItems = items
    },
    getSuspectsItemsError (state, error) {
        state.isLoadingSuspects = false
        state.isLoadSuspects = false
        state.messageError = error
        state.suspectsItems = []
    }
}

const actions = {
    async getSuspectsItems({commit}) {
        commit('getSuspectsItemsStarted')
        let res = await api.getSuspects
        if(res.status == 200) {
            commit('getSuspectsItemsSuccess', res.data.Value)
        }
        else{
            commit('getSuspectsItemsStarted')
        }
        console.log(res)
    }
}

export default {
    state,
    getters,
    mutations,
    actions
}
